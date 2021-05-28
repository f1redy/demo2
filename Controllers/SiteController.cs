using gestion.site.Core;
using gestion.site.Data;
using gestion.site.Model.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace gestion.site.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly GestionContext _db;
        private readonly Configuracion _config;
        public SiteController(GestionContext db, IOptions<Configuracion> config)
        {
            _db = db;
            _config = config.Value;
        }

        [Route("login")]
        [HttpPost]
        public ActionResult<Access> Login(Login _info)
        {
            var ret = _db.especialista.FirstOrDefault(x => x.usuario.ToLower() == _info.usuario.ToLower());
            if (ret == null)
            {
                return NotFound("Usuario no existe");
            }
            if (!Utils.ValidarTexto(_info.clave, ret.contrasena))
            {
                return NotFound("Contraseña incorrecta");
            } 
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Llave);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, ret.especialista_desc),
                    new Claim(ClaimTypes.Role, ret.rol_cod),
                    new Claim(ClaimTypes.NameIdentifier, ret.usuario),
                    new Claim(ClaimTypes.PrimarySid, ret.especialista_id.ToString()   )
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);
            var model = new Access();
            model.usuario = ret.usuario;
            model.nombre = ret.especialista_desc;
            model.token = handler.WriteToken(token);
            return model;
        }

    }
}

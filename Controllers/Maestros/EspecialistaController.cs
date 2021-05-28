using gestion.site.Core;
using gestion.site.Data;
using gestion.site.Model.Site;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestion.site.Controllers.Maestros
{
    [Authorize(Roles = RolAcceso.ADMIN)]
    [Route("api/maestros/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Never")]
    public class EspecialistaController : ControllerBase
    {
        private readonly GestionContext _db;

        public EspecialistaController(GestionContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<dynamic> Get()
        {
            var ret = await (from x in _db.especialista
                             select new
                             {
                                 x.especialista_id,
                                 x.especialista_desc,
                                 x.usuario,
                                 x.estatus,
                                 x.rol_cod
                             }).ToListAsync();
            return ret;
        }

        [HttpGet("{id}")]
        public async Task<dynamic> Get(int id)
        {
            var ret = await (from x in _db.especialista
                             where x.especialista_id == id
                             select new
                             {
                                 x.especialista_id,
                                 x.especialista_desc,
                                 x.usuario,
                                 x.estatus,
                                 x.rol_cod,
                                 x.rol_codNavigation.rol_desc,
                                 x.correo
                             }).FirstOrDefaultAsync();
            if (ret == null)
            {
                return NotFound();
            }
            return ret;
        }

        [HttpPost()]
        public async Task Post(especialista model)
        {            
            model.contrasena = Utils.CifrarTexto(model.contrasena);
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, especialista model)
        {
            var ret = await _db.especialista.AnyAsync(x => x.especialista_id == id);
            
            if (!ret)
            {
                return NotFound();
            }

            _db.Entry(model).State = EntityState.Modified;

            if (model.contrasena == null)
            {
                _db.Entry(model).Property(x => x.contrasena).IsModified = false;
            }
            else
            {
                model.contrasena = Utils.CifrarTexto(model.contrasena);
            }                        

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ret = await _db.especialista.FindAsync(id);
            if (ret==null)
            {
                return NotFound();
            }
            _db.especialista.Remove(ret);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}

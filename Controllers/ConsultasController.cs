using gestion.site.Core;
using gestion.site.Data;
using gestion.site.Model.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ConsultasController : ControllerBase
    {
        private readonly GestionContext _db;

        public ConsultasController(GestionContext db)
        {
            _db = db;
        }

        [Route("roles")]
        [HttpGet]
        public async Task<dynamic> Roles()
        {
            return await (from x in _db.rol
                          select new
                          {
                              id = x.rol_cod,
                              etiqueta = x.rol_cod + " - " + x.rol_desc,
                              desc = x.rol_desc
                          }).ToListAsync();
        }

        [Route("solicitantes")]
        [HttpGet]
        public async Task<dynamic> Solicitantes()
        {
            return await (from x in _db.solicitante                          
                          select new
                          {
                              id = x.solicitante_id,
                              etiqueta = x.solicitante_id.ToString() + " - " + x.solicitante_desc,
                              desc = x.solicitante_desc
                          }).ToListAsync();
        }

        [Route("especialistas")]
        [HttpGet]
        public async Task<dynamic> Especialista()
        {
            return await (from x in _db.especialista
                          select new
                          {
                              id = x.especialista_id,
                              etiqueta = x.especialista_id.ToString() + " - " + x.especialista_desc,
                              desc = x.especialista_desc
                          }).ToListAsync();
        }

        [Route("criticidad")]
        [HttpGet]
        public async Task<dynamic> Criticidad()
        {
            return await (from x in _db.criticidad
                          select new
                          {
                              id = x.criticidad_id,
                              etiqueta = x.criticidad_id.ToString() + " - " + x.criticidad_desc,
                              desc = x.criticidad_desc
                          }).ToListAsync();
        }

        [Route("areas")]
        [HttpGet]
        public async Task<dynamic> Area()
        {
            return await (from x in _db.area
                          select new
                          {
                              id = x.area_id,
                              etiqueta = x.area_id.ToString() + " - " + x.area_desc,
                              desc = x.area_desc
                          }).ToListAsync();
        }

        [Route("estado-inicial")]
        [HttpGet]
        public async Task<dynamic> EstadoInicial()
        {
            return await (from x in _db.estado.FromSqlRaw("select * from estado_inicial")
                          select new
                          {
                              id = x.estado_id,
                              etiqueta = x.estado_id.ToString() + " - " + x.estado_desc,
                              desc = x.estado_desc
                          }).ToListAsync();
        }
        [Route("estado-final")]
        [HttpGet]
        public async Task<dynamic> EstadoFinal()
        {
            return await (from x in _db.estado.FromSqlRaw("select * from estado_final")
                          select new
                          {
                              id = x.estado_id,
                              etiqueta = x.estado_id.ToString() + " - " + x.estado_desc,
                              desc = x.estado_desc
                          }).ToListAsync();
        }

        [Route("estado-siguiente/{id}")]
        [HttpGet]
        public async Task<dynamic> EstadoSiguiente(int id)
        {
            return await (from x in _db.relacion_estado
                          where x.estado_inicial_id==id
                          select new
                          {
                              id = x.estado_final_id,
                              etiqueta = x.estado_final_id.ToString() + " - " + x.estado_final.estado_desc,
                              desc = x.estado_final.estado_desc
                          }).ToListAsync();
        }

    }
}

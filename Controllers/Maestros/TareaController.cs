using gestion.site.Core;
using gestion.site.Data;
using gestion.site.Model.Site;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace gestion.site.Controllers.Maestros
{
    [Authorize()]
    [Route("api/maestros/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Never")]
    public class TareaController : ControllerBase
    {
        private readonly GestionContext _db;

        public TareaController(GestionContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<dynamic> Get()
        {


            var ret = await (from x in _db.tarea
                             select new
                             {
                                 x.tarea_id,
                                 x.tarea_desc,
                                 x.criticidad.criticidad_desc,
                                 x.solicitante.solicitante_desc,
                                 x.creador.especialista_desc,
                                 x.estado.estado_desc
                             }).ToListAsync();
            return ret;
        }

        [HttpGet("mis-tareas")]
        public async Task<dynamic> GetPropias()
        {
            var Id = User.FindFirstValue(ClaimTypes.PrimarySid);
            if (Id == null)
            {
                return NotFound();
            }
            var ret = await (from x in _db.tarea
                             where x.creador_id.ToString() == Id
                             select new
                             {
                                 x.tarea_id,
                                 x.tarea_desc,
                                 x.criticidad.criticidad_desc,
                                 x.solicitante.solicitante_desc,
                                 x.creador.especialista_desc,
                                 x.estado.estado_desc
                             }).ToListAsync();
            return ret;
        }


        [HttpGet("{id}")]
        public async Task<dynamic> Get(int id)
        {
            var ret = await (from x in _db.tarea
                             where x.tarea_id == id
                             select new
                             {
                                 x.tarea_id,
                                 x.tarea_desc,
                                 x.area.area_desc,
                                 x.area_id,
                                 creador_desc = x.creador.especialista_desc,
                                 x.creador_id,
                                 x.criticidad.criticidad_desc,
                                 x.criticidad_id,
                                 x.estado.estado_desc,
                                 x.estado_id,
                                 x.fecha_asignacion,
                                 x.fecha_compromiso,
                                 x.fecha_solicitud,
                                 x.fecha_solucion,
                                 x.solicitante.solicitante_desc,
                                 x.solicitante_id,
                                 especialistas = (from y in x.tarea_especialista.ToList()
                                                  select new
                                                  {
                                                      y.especialista.especialista_desc,
                                                      y.especialista_id
                                                  }).ToList()
                             }).FirstOrDefaultAsync();
            if (ret == null)
            {
                return NotFound();
            }
            return ret;
        }

        [HttpPost()]
        public async Task<dynamic> Post(tarea model)
        {
            model.fecha_solicitud = DateTime.UtcNow;
            var Id = User.FindFirstValue(ClaimTypes.PrimarySid);
            if (Id == null)
            {
                return NotFound();
            }
            model.creador_id = int.Parse(Id);
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            var solicitante = await _db.solicitante.FindAsync(model.solicitante_id);
            Utils.EnviarCorreo(solicitante.correo,
                "Tarea Creada",
                $"<h2>Se ha creado la Tarea # {model.tarea_id} </h2>" +
                $"Descripcion: {model.tarea_desc} <br/>",             
                true);
            return Ok();
        }
    }
}

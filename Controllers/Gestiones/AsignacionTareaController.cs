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
    [Route("api/gestiones/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Never")]
    public class AsignacionTareaController : ControllerBase
    {
        private readonly GestionContext _db;

        public AsignacionTareaController(GestionContext db)
        {
            _db = db;
        }

        [HttpGet("sin-asignar")]
        public async Task<dynamic> Get()
        {
            var ret = await (from x in _db.tarea
                             where x.estado.estado_desc== "Registrada"
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

        [HttpGet("en-proceso")]
        public async Task<dynamic> GetPropias()
        {

            var ret = await (from x in _db.tarea
                             where x.estado.estado_desc== "Asignada" || x.estado.estado_desc == "En Proceso"
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
        public async Task<dynamic> Post(tarea_especialista model)
        {

            var MAX_CARGA = (await _db.configuracion.FindAsync("CARGA-MAX")).valor_numerico;

            var cant = (from x in _db.tarea_especialista
                        where x.especialista_id == model.especialista_id
                        && (x.tarea.estado.estado_desc != "Solucionada" && x.tarea.estado.estado_desc != "Anulada")
                        select x.tarea_id
                        ).Count();

            if (cant >= MAX_CARGA)
            {
                return NotFound("Se llego a la carga maxima de trabajo " + MAX_CARGA.ToString());
            }
            var tarea = await _db.tarea.Include("estado").FirstAsync(x=> x.tarea_id== model.tarea_id);
            var estado = await _db.estado.FirstAsync(y => y.estado_desc== "Asignada");
            if (tarea.estado.estado_desc == "Registrada")
            {
                tarea.estado_id = estado.estado_id;
            }
            await _db.AddAsync(model);
            
            await _db.SaveChangesAsync();

            var solicitante = await _db.solicitante.FindAsync(tarea.solicitante_id);
            var especialista = await _db.especialista.FindAsync(model.especialista_id);

            Utils.EnviarCorreo(solicitante.correo,
                "Tarea Asignada",
                $"<h2>Se ha asignado el especialista {especialista.especialista_desc} a  la Tarea # {model.tarea_id} </h2>" ,
                true);

            Utils.EnviarCorreo(especialista.correo,
                "Tarea Asignada",
                $"<h2>Se ha asignado la Tarea # {model.tarea_id} </h2>",
                true);

            return Ok();

        }
    }
}

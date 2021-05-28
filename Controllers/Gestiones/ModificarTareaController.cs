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
    public class ModificarTareaController : ControllerBase
    {
        private readonly GestionContext _db;

        public ModificarTareaController(GestionContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<dynamic> Get()
        {
            var ret = await (from x in _db.tarea
                             where (x.estado.estado_desc != "Solucionada" && x.estado.estado_desc != "Anulada")
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
        [Authorize(Roles = "SUPERVISOR")]
        [HttpDelete("asignacion/{tarea}/{especialista}")]
        public async Task<dynamic> EliminarAsignacion(long tarea, int especialista)
        {
            var cant = (from x in _db.tarea_especialista
                        where x.tarea_id == tarea
                        select x.especialista_id
                        ).Count();
            if (cant == 1)
            {
                return NotFound("No se puede quedar la tarea sin responsables");
            }
            var ret = await _db.tarea_especialista.FindAsync(tarea, especialista);
            if (ret == null)
            {
                return NotFound();
            }
            _db.tarea_especialista.Remove(ret);
            await _db.SaveChangesAsync();
            var t = await _db.tarea.FindAsync(tarea);
            var solicitante = await _db.solicitante.FindAsync(t.solicitante_id);
            var esp = await _db.especialista.FindAsync(especialista);
            Utils.EnviarCorreo(solicitante.correo,
                "Tarea Asignada",
                $"<h2>Se ha asignado el especialista {esp.especialista_desc} a  la Tarea # {tarea} </h2>",
                true);
            Utils.EnviarCorreo(esp.correo,
                "Tarea Asignada",
                $"<h2>Se ha asignado la Tarea # {tarea} </h2>",
                true);

            return Ok();
        }
        [Authorize(Roles = "DIRECTOR")]
        [HttpPut("anular/{id}")]
        public async Task<dynamic> Anular(long id)
        {

            var tarea = await _db.tarea.Include("estado").FirstAsync(x => x.tarea_id == id);

            if (tarea == null)
            {
                return NotFound();
            }

            var estado = await _db.estado.FirstAsync(y => y.estado_desc == "Anulada");
            tarea.estado_id = estado.estado_id;
            await _db.SaveChangesAsync();

            var solicitante = await _db.solicitante.FindAsync(tarea.solicitante_id);            
            Utils.EnviarCorreo(solicitante.correo,
                "Tarea Anulado",
                $"<h2>Se ha anulado la Tarea # {id} </h2>",
                true);

            var especialistas = await _db.tarea_especialista.Where(x => x.tarea_id == id).ToListAsync();
            foreach (var i in especialistas)
            {
                var especialista = await _db.especialista.FindAsync(i.tarea_id);
                Utils.EnviarCorreo(especialista.correo,
                    "Tarea Anulado",
                    $"<h2>Se ha anulado la Tarea # {id} </h2>",
                    true);

            }

            return Ok();
        }
        [Authorize(Roles = "ESPECIALISTA")]
        [HttpPut("estado/{id}/{estado}")]
        public async Task<dynamic> Estado(long id, int estado)
        {

            var tarea = await _db.tarea.Include("estado").FirstAsync(x => x.tarea_id == id);

            if (tarea == null)
            {
                return NotFound();
            }

            tarea.estado_id = estado;

            await _db.SaveChangesAsync();

            var e = await _db.estado.FindAsync(estado);
            var solicitante = await _db.solicitante.FindAsync(tarea.solicitante_id);
          
            Utils.EnviarCorreo(solicitante.correo,
                "Actualizacion de Tarea",
                $"<h2>Se actualizado la Tarea # {id}, al estado {e.estado_desc} </h2>",
                true);
            return Ok();
        }
    }
}

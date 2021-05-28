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
    public class ReportesController : ControllerBase
    {
        private readonly GestionContext _db;

        public ReportesController(GestionContext db)
        {
            _db = db;
        }
        
        [HttpGet("por-criticidad/{criticidad}/{fechai}/{fechaf}")]
        public async Task<dynamic> PorCriticidad(string criticidad, DateTime fechai, DateTime fechaf)
        {

            var ret = await (from x in _db.tarea
                             where x.fecha_solicitud.Date >= fechai.Date && x.fecha_solicitud.Date <= fechaf.Date
                             && (criticidad == x.criticidad_id.ToString() || criticidad == "*")
                             select new
                             {
                                 tarea_id = x.tarea_id,
                                 tarea_desc = x.tarea_desc,                                 
                                 area_desc = x.area.area_desc,
                                 area_id = x.area_id,
                                 creador_desc = x.creador.especialista_desc,
                                 creador_id = x.creador_id,
                                 criticidad_desc = x.criticidad.criticidad_desc,
                                 criticidad_id = x.criticidad_id,
                                 estado_desc = x.estado.estado_desc,
                                 estado_id = x.estado_id,
                                 fecha_asignacion = x.fecha_asignacion,
                                 fecha_compromiso = x.fecha_compromiso,
                                 fecha_solicitud = x.fecha_solicitud,
                                 fecha_solucion = x.fecha_solucion,
                                 solicitante_desc = x.solicitante.solicitante_desc,
                                 solicitante_id = x.solicitante_id,
                                 especialistas = string.Join(", ", (from y in x.tarea_especialista.ToList()
                                                                    select y.especialista.especialista_desc
                                                      ).ToList())
                             }).ToListAsync();
            return ret;
        }

        [HttpGet("en-tiempo/{fechai}/{fechaf}")]
        public async Task<dynamic> EnTiempo(DateTime fechai, DateTime fechaf)
        {
            var ret = await (from x in _db.tarea
                             where x.fecha_solicitud.Date >= fechai.Date && x.fecha_solicitud.Date <= fechaf.Date
                             && x.fecha_solucion != null
                             && ((DateTime)x.fecha_solucion).Date <= x.fecha_compromiso.Date
                             select new
                             {
                                 tarea_id = x.tarea_id,
                                 tarea_desc = x.tarea_desc,
                                 area_desc = x.area.area_desc,
                                 area_id = x.area_id,
                                 creador_desc = x.creador.especialista_desc,
                                 creador_id = x.creador_id,
                                 criticidad_desc = x.criticidad.criticidad_desc,
                                 criticidad_id = x.criticidad_id,
                                 estado_desc = x.estado.estado_desc,
                                 estado_id = x.estado_id,
                                 fecha_asignacion = x.fecha_asignacion,
                                 fecha_compromiso = x.fecha_compromiso,
                                 fecha_solicitud = x.fecha_solicitud,
                                 fecha_solucion = x.fecha_solucion,
                                 solicitante_desc = x.solicitante.solicitante_desc,
                                 solicitante_id = x.solicitante_id,
                                 especialistas = string.Join(", ", (from y in x.tarea_especialista.ToList()
                                                                    select y.especialista.especialista_desc
                                                      ).ToList())
                             }).ToListAsync();
            return ret;
        }

        [HttpGet("fuera-tiempo/{fechai}/{fechaf}")]
        public async Task<dynamic> FueraTiempo(DateTime fechai, DateTime fechaf)
        {
            var ret = await (from x in _db.tarea
                             where x.fecha_solicitud.Date >= fechai.Date && x.fecha_solicitud.Date <= fechaf.Date
                             && x.fecha_solucion != null
                             && ((DateTime)x.fecha_solucion).Date > x.fecha_compromiso.Date
                             select new
                             {
                                 tarea_id = x.tarea_id,
                                 tarea_desc = x.tarea_desc,
                                 area_desc = x.area.area_desc,
                                 area_id = x.area_id,
                                 creador_desc = x.creador.especialista_desc,
                                 creador_id = x.creador_id,
                                 criticidad_desc = x.criticidad.criticidad_desc,
                                 criticidad_id = x.criticidad_id,
                                 estado_desc = x.estado.estado_desc,
                                 estado_id = x.estado_id,
                                 fecha_asignacion = x.fecha_asignacion,
                                 fecha_compromiso = x.fecha_compromiso,
                                 fecha_solicitud = x.fecha_solicitud,
                                 fecha_solucion = x.fecha_solucion,
                                 solicitante_desc = x.solicitante.solicitante_desc,
                                 solicitante_id = x.solicitante_id,
                                 especialistas = string.Join(", ", (from y in x.tarea_especialista.ToList()
                                                                    select y.especialista.especialista_desc
                                                      ).ToList())
                             }).ToListAsync();
            return ret;
        }

        [HttpGet("por-responsable/{especialista}/{fechai}/{fechaf}")]
        public async Task<dynamic> PorResposnable(string especialista, DateTime fechai, DateTime fechaf)
        {
            var ret = await (from x in _db.tarea_especialista
                             where x.tarea.fecha_solicitud.Date >= fechai.Date && x.tarea.fecha_solicitud.Date <= fechaf.Date
                              && (especialista == x.especialista_id.ToString() || especialista == "*")
                             select new
                             {
                                 x.tarea_id,
                                 x.especialista_id,
                                 x.especialista.especialista_desc,
                                 x.tarea.estado.estado_desc,
                             }).ToListAsync();

            var d = (from x in ret
                     group x by new { x.especialista_id, x.especialista_desc } into g
                     select new
                     {
                         g.Key.especialista_id,
                         g.Key.especialista_desc,
                         total = g.Count(),
                         solucionada = g.Count(i => i.estado_desc == "Solucionada"),
                         anulada = g.Count(i => i.estado_desc == "Anulada"),
                         en_proceso = g.Count(i => i.estado_desc == "En Proceso"),
                         asignada = g.Count(i => i.estado_desc == "Asignada")
                     });

            return d;
        }


        [HttpGet("por-area/{area}/{fechai}/{fechaf}")]
        public async Task<dynamic> PorArea(string area, DateTime fechai, DateTime fechaf)
        {
            var ret = await (from x in _db.tarea
                             where x.fecha_solicitud.Date >= fechai.Date && x.fecha_solicitud.Date <= fechaf.Date
                              && (area == x.area_id.ToString() || area == "*")
                             select new
                             {
                                 x.tarea_id,
                                 x.area_id,
                                 x.area.area_desc,
                                 x.estado.estado_desc,
                             }).ToListAsync();

            var d = (from x in ret
                     group x by new { x.area_id, x.area_desc } into g
                     select new
                     {
                         g.Key.area_id,
                         g.Key.area_desc,
                         total = g.Count(),
                         solucionada = g.Count(i => i.estado_desc == "Solucionada"),
                         anulada = g.Count(i => i.estado_desc == "Anulada"),
                         registrada = g.Count(i => i.estado_desc == "Registrada"),
                         en_proceso = g.Count(i => i.estado_desc == "En Proceso"),
                         asignada = g.Count(i => i.estado_desc == "Asignada")
                     });

            return d;
        }

    }
}


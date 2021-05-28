using gestion.site.Core;
using gestion.site.Data;
using gestion.site.Model.Site;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "ADMIN")]
    [Route("api/gestiones/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Never")]
    public class ExportacionController : ControllerBase
    {
        private readonly GestionContext _db;

        public ExportacionController(GestionContext db)
        {
            _db = db;
        }

        [HttpGet("{fechai}/{fechaf}")]
        public async Task<dynamic> Get(DateTime fechai, DateTime fechaf)
        {
            var ret = await (from x in _db.tarea
                             where x.fecha_solicitud.Date >= fechai.Date && x.fecha_solicitud.Date <= fechaf.Date
                             select new
                             {
                                 Tarea_id = x.tarea_id,
                                 Tarea_desc = x.tarea_desc,
                                 Area_desc = x.area.area_desc,
                                 Area_id = x.area_id,
                                 Creador_desc = x.creador.especialista_desc,
                                 Creador_id = x.creador_id,
                                 Criticidad_desc = x.criticidad.criticidad_desc,
                                 Criticidad_id = x.criticidad_id,
                                 Estado_desc = x.estado.estado_desc,
                                 Estado_id = x.estado_id,
                                 Fecha_asignacion = x.fecha_asignacion,
                                 Fecha_compromiso = x.fecha_compromiso,
                                 Fecha_solicitud = x.fecha_solicitud,
                                 Fecha_solucion = x.fecha_solucion,
                                 Solicitante_desc = x.solicitante.solicitante_desc,
                                 Solicitante_id = x.solicitante_id,
                                 especialistas = string.Join(", ",(from y in x.tarea_especialista.ToList()
                                                  select y.especialista.especialista_desc
                                                      ).ToList())
                             }).ToListAsync();
           
            return ret;
        }
    }
}

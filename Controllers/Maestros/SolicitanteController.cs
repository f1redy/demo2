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
    [Authorize(Roles = "ADMIN")]
    [Route("api/maestros/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Never")]
    public class SolicitanteController : ControllerBase
    {
        private readonly GestionContext _db;

        public SolicitanteController(GestionContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<dynamic> Get()
        {
            var ret = await (from x in _db.solicitante
                             select new
                             {
                                 x.solicitante_id,
                                 x.solicitante_desc,                                 
                                 x.estatus,                                 
                             }).ToListAsync();
            return ret;
        }

        [HttpGet("{id}")]
        public async Task<dynamic> Get(int id)
        {
            var ret = await (from x in _db.solicitante
                             where x.solicitante_id == id
                             select new
                             {
                                 x.solicitante_id,
                                 x.solicitante_desc,    
                                 x.correo,
                                 x.estatus,                                 
                             }).FirstOrDefaultAsync();
            if (ret == null)
            {
                return NotFound();
            }
            return ret;
        }

        [HttpPost()]
        public async Task<ActionResult> Post(solicitante model)
        {                      
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, solicitante model)
        {
            var ret = await _db.solicitante.AnyAsync(x => x.solicitante_id == id);            
            if (!ret)
            {
                return NotFound();
            }
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ret = await _db.solicitante.FindAsync(id);

            if (ret==null)
            {
                return NotFound();
            }
            _db.solicitante.Remove(ret);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}

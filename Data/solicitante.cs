using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class solicitante
    {
        public solicitante()
        {
            tarea = new HashSet<tarea>();
        }

        public int solicitante_id { get; set; }
        public string solicitante_desc { get; set; }
        public string correo { get; set; }
        public bool estatus { get; set; }

        public virtual ICollection<tarea> tarea { get; set; }
    }
}

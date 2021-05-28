using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class criticidad
    {
        public criticidad()
        {
            tarea = new HashSet<tarea>();
        }

        public int criticidad_id { get; set; }
        public string criticidad_desc { get; set; }
        public int nivel { get; set; }
        public int alerta { get; set; }
        public bool estatus { get; set; }

        public virtual ICollection<tarea> tarea { get; set; }
    }
}

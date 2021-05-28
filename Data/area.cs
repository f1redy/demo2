using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class area
    {
        public area()
        {
            tarea = new HashSet<tarea>();
        }

        public int area_id { get; set; }
        public string area_desc { get; set; }
        public bool estatus { get; set; }

        public virtual ICollection<tarea> tarea { get; set; }
    }
}

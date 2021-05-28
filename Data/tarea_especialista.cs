using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class tarea_especialista
    {
        public long tarea_id { get; set; }
        public int especialista_id { get; set; }

        public virtual especialista especialista { get; set; }
        public virtual tarea tarea { get; set; }
    }
}

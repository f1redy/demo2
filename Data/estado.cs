using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class estado
    {
        public estado()
        {
            relacion_estadoestado_final = new HashSet<relacion_estado>();
            relacion_estadoestado_inicial = new HashSet<relacion_estado>();
            tarea = new HashSet<tarea>();
        }

        public int estado_id { get; set; }
        public string estado_desc { get; set; }
        public bool estatus { get; set; }

        public virtual ICollection<relacion_estado> relacion_estadoestado_final { get; set; }
        public virtual ICollection<relacion_estado> relacion_estadoestado_inicial { get; set; }
        public virtual ICollection<tarea> tarea { get; set; }
    }
}

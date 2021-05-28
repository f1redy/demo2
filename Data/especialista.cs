using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class especialista
    {
        public especialista()
        {
            tarea = new HashSet<tarea>();
            tarea_especialista = new HashSet<tarea_especialista>();
        }

        public int especialista_id { get; set; }
        public string especialista_desc { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string rol_cod { get; set; }
        public string correo { get; set; }
        public bool estatus { get; set; }

        public virtual rol rol_codNavigation { get; set; }
        public virtual ICollection<tarea> tarea { get; set; }
        public virtual ICollection<tarea_especialista> tarea_especialista { get; set; }
    }
}

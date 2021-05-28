using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class rol
    {
        public rol()
        {
            especialista = new HashSet<especialista>();
        }

        public string rol_cod { get; set; }
        public string rol_desc { get; set; }
        public bool estatus { get; set; }

        public virtual ICollection<especialista> especialista { get; set; }
    }
}

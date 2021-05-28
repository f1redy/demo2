using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class relacion_estado
    {
        public int estado_inicial_id { get; set; }
        public int estado_final_id { get; set; }
        public bool estatus { get; set; }

        public virtual estado estado_final { get; set; }
        public virtual estado estado_inicial { get; set; }
    }
}

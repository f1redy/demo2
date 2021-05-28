using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class configuracion
    {
        public string configuracion_cod { get; set; }
        public string configuracion_desc { get; set; }
        public decimal? valor_numerico { get; set; }
        public string valor_texto { get; set; }
        public bool estatus { get; set; }
    }
}

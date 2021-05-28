using System;
using System.Collections.Generic;

#nullable disable

namespace gestion.site.Data
{
    public partial class tarea
    {
        public tarea()
        {
            tarea_especialista = new HashSet<tarea_especialista>();
        }

        public long tarea_id { get; set; }
        public string tarea_desc { get; set; }
        public int area_id { get; set; }
        public int solicitante_id { get; set; }
        public int criticidad_id { get; set; }
        public int creador_id { get; set; }
        public int estado_id { get; set; }
        public DateTime fecha_solicitud { get; set; }
        public DateTime? fecha_asignacion { get; set; }
        public DateTime fecha_compromiso { get; set; }
        public DateTime? fecha_solucion { get; set; }

        public virtual area area { get; set; }
        public virtual especialista creador { get; set; }
        public virtual criticidad criticidad { get; set; }
        public virtual estado estado { get; set; }
        public virtual solicitante solicitante { get; set; }
        public virtual ICollection<tarea_especialista> tarea_especialista { get; set; }
    }
}

using System.Collections.Generic;

namespace SGDE.Domain.Entities
{
    public class Partida : BaseEntity
    {
        public string Codigo { get; set; }
        public string Orden { get; set; }
        public string Descripcion { get; set; }
        public string Unidades { get; set; }
        public double? UnidadesCertificacionAnterior { get; set; }
        public double? UnidadesCertificacionActual { get; set; }
        public double? UnidadesPresupuesto { get; set; }
        public double? PresupuestoCapitulo { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Partida> SubCapitulos { get; set; } = new HashSet<Partida>();
    }
}

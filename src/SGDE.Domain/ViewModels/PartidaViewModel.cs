namespace SGDE.Domain.ViewModels
{
    public class PartidaViewModel : BaseEntityViewModel
    {
        public string codigo { get; set; }
        public string orden { get; set; }
        public string descripcion { get; set; }
        public string unidades { get; set; }
        public double? unidadesCertificacionAnterior { get; set; }
        public double? unidadesCertificacionActual { get; set; }
        public double? unidadesPresupuesto { get; set; }
        public double? presupuestoCapitulo { get; set; }
        public string type { get; set; }
    }
}

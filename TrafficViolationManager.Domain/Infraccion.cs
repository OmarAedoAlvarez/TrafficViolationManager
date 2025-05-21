using TrafficViolationManager.Domain;

namespace TrafficViolationManager.Domain
{
    public class Infraccion
    {
        public int InfraccionId { get; set; }
        public string Descripcion { get; set; }
        public decimal MontoMulta { get; set; }
        public GravedadInfraccion Gravedad { get; set; }
        public int Puntos { get; set; }
    }
}

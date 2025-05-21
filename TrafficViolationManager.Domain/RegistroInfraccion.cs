using System;

namespace TrafficViolationManager.Domain
{
    public class RegistroInfraccion
    {
        public DateTime Fecha { get; set; }
        public int VehiculoId { get; set; }
        public int ConductorId { get; set; }
        public int InfraccionId { get; set; }

        // Puedes agregar referencias opcionales a otras entidades si usas ORM:
        // public Vehiculo Vehiculo { get; set; }
        // public Conductor Conductor { get; set; }
        // public Infraccion Infraccion { get; set; }
    }
}

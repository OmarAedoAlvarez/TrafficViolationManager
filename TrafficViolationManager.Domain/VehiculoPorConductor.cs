using System;

namespace TrafficViolationManager.Domain
{
    public class VehiculoPorConductor
    {
        public int VehiculoId { get; set; }
        public int ConductorId { get; set; }
        public DateTime FechaAdquisicion { get; set; }
    }
}

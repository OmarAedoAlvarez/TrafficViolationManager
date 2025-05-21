namespace TrafficViolationManager.Domain
{
    public class Conductor
    {
        public int ConductorId { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Nombres { get; set; }
        public string NumLicencia { get; set; }
        public int TipoLicenciaId { get; set; }
        public int PuntosAcumulados { get; set; }

        // Relación (opcional) para facilitar navegación:
        public TipoLicencia TipoLicencia { get; set; }
    }
}

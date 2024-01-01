namespace N5Challenge.Domain.Entities
{
    public class ElasticPermission
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public int TipoPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }
        public string TipoPermisoDescripcion { get; set; } = string.Empty;
    }
}

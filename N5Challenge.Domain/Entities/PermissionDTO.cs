namespace N5Challenge.Domain.Entities
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public string TipoPermiso { get; set; } = string.Empty;
    }
}

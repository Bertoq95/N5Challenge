namespace N5Challenge.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public int TipoPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }


        public PermissionTypes PermissionType { get; set; } = new();
    }
}

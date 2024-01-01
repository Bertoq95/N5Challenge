namespace N5Challenge.Domain.Entities
{
    public class PermissionTypes
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new();

    }
}

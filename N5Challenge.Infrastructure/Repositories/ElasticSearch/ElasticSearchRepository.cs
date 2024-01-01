using N5Challenge.Domain.Entities;

namespace N5Challenge.Infrastructure.Repositories.ElasticSearch
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        public Task<ElasticPermission> CreateElasticPermission(Permission permission, PermissionTypes permissionType)
        {
            var elasticPermission = new ElasticPermission
            {
                Id = permission.Id,
                NombreEmpleado = permission.NombreEmpleado,
                ApellidoEmpleado = permission.ApellidoEmpleado,
                TipoPermiso = permission.TipoPermiso,
                FechaPermiso = permission.FechaPermiso,
                TipoPermisoDescripcion = permissionType.Descripcion
            };

            return Task.FromResult(elasticPermission);
        }


    }
}

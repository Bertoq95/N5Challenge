using N5Challenge.Domain.Entities;

namespace N5Challenge.Infrastructure.Repositories.ElasticSearch
{
    public interface IElasticSearchRepository
    {
        public Task<ElasticPermission> CreateElasticPermission(Permission permission, PermissionTypes permissionType);


    }
}

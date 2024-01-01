using N5Challenge.Domain.Entities;

namespace N5Challenge.Infrastructure.Repositories.GetPermission
{
    public interface IGetPermissionRepository
    {
        Task<List<PermissionDTO>> GetAllPermissions();
    }
}

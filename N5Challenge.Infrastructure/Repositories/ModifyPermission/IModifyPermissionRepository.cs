using N5Challenge.Domain.Entities;
using N5Challenge.Domain.Request;

namespace N5Challenge.Infrastructure.Repositories.ModifyPermission
{
    public interface IModifyPermissionRepository
    {
        Task<PermissionDTO> ModifyPermission(int id, PermissionRequest request);

    }
}

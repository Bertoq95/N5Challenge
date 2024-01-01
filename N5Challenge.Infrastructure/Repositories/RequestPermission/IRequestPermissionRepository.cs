using N5Challenge.Domain.Entities;
using N5Challenge.Domain.Request;

namespace N5Challenge.Infrastructure.Repositories.RequestPermission
{
    public interface IRequestPermissionRepository
    {
        Task<PermissionDTO> ProcessRequestPermission(PermissionRequest request);

    }
}

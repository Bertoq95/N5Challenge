using MediatR;
using N5Challenge.Application.useCase.Get.Command;
using N5Challenge.Application.useCase.Put.Command;
using N5Challenge.Domain.Entities;
using N5Challenge.Infrastructure.Repositories.GetPermission;

namespace N5Challenge.Application.useCase.Get.Handler
{
    public class GetPermissionCommandHandler : IRequestHandler<GetPermissionCommand, List<PermissionDTO>>
    {
        private readonly IGetPermissionRepository _repository;

        public GetPermissionCommandHandler(IGetPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PermissionDTO>> Handle (GetPermissionCommand request, CancellationToken cancellationToken)
        {
            var permissions = await _repository.GetAllPermissions();
            return permissions;

        }
    }
}

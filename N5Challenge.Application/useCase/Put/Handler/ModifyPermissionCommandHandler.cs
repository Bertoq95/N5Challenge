using MediatR;
using N5Challenge.Application.useCase.Put.Command;
using N5Challenge.Infrastructure.Repositories.ModifyPermission;

namespace N5Challenge.Application.useCase.Put.Handler
{
    public class ModifyPermissionCommandHandler : IRequestHandler<ModifyPermissionCommand, Unit>
    {
        private readonly IModifyPermissionRepository _repository;

        public ModifyPermissionCommandHandler(IModifyPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            await _repository.ModifyPermission(request.Id, request.Request);
            return Unit.Value;
        }
    }
}

using MediatR;
using N5Challenge.Application.useCase.Post.Command;
using N5Challenge.Infrastructure.Repositories.RequestPermission;

namespace N5Challenge.Application.useCase.Post.Handler
{
    public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, Unit>
    {
        private readonly IRequestPermissionRepository _repository;

        public RequestPermissionCommandHandler(IRequestPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            await _repository.ProcessRequestPermission( request.Request);
            return Unit.Value;
        }
    }
}

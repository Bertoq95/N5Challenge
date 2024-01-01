using MediatR;
using N5Challenge.Domain.Request;

namespace N5Challenge.Application.useCase.Post.Command
{
    public class RequestPermissionCommand : IRequest<Unit>
    {
        public PermissionRequest Request { get; set; } = new();

    }
}

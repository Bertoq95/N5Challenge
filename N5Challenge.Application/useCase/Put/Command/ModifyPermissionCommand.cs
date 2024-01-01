using MediatR;
using N5Challenge.Domain.Request;

namespace N5Challenge.Application.useCase.Put.Command
{
    public class ModifyPermissionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public PermissionRequest Request { get; set; } = new();
    }
}

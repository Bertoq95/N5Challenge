using MediatR;
using N5Challenge.Domain.Entities;

namespace N5Challenge.Application.useCase.Get.Command
{
    public class GetPermissionCommand : IRequest<List<PermissionDTO>>
    {

    }
}

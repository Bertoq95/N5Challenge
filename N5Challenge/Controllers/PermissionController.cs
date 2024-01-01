using Microsoft.AspNetCore.Mvc;
using MediatR;
using N5Challenge.Domain.Request;
using N5Challenge.Application.useCase.Put.Command;
using N5Challenge.Application.useCase.Get.Command;
using N5Challenge.Application.useCase.Post.Command;

namespace N5Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyPermission(int id, [FromBody] PermissionRequest request)
        {
            try
            {
                var command = new ModifyPermissionCommand { Id = id, Request = request };
                await _mediator.Send(command);
                return Ok("Permiso modificado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al modificar el permiso: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermission()
        {
            try
            {
                var command = new GetPermissionCommand { };

                var permissions = await _mediator.Send(command);
                return Ok(permissions);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los permisos: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RequestPermission([FromBody] PermissionRequest request)
        {
            try
            {
                var command = new RequestPermissionCommand { Request = request };
                await _mediator.Send(command);
                return Ok("Permiso creado correctamente");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el permiso: {ex.Message}");
            }
        }
    }
}

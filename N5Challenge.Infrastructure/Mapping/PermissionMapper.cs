using N5Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Infrastructure.Mapping
{
    public class PermissionMapper
    {
        public PermissionDTO MapPermissionToDTO(Permission permission)
        {
            return new PermissionDTO
            {
                NombreEmpleado = permission.NombreEmpleado,
                ApellidoEmpleado = permission.ApellidoEmpleado,
                TipoPermiso = permission.PermissionType.Descripcion
            };
        }
    }
}

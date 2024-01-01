using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Domain.Entities;
using N5Challenge.Domain.Request;
using N5Challenge.Infrastructure.EntityFramework;
using N5Challenge.Infrastructure.Kafka.Producer;
using N5Challenge.Infrastructure.Mapping;
using N5Challenge.Infrastructure.Repositories.ElasticSearch;
using Nest;
using Serilog;
using System.Security;

namespace N5Challenge.Infrastructure.Repositories.ModifyPermission
{
    public class ModifyPermissionRepository : IModifyPermissionRepository
    {
        private readonly AppDbContext _context;
        private readonly PermissionMapper _mapper;
        private readonly IElasticClient _elasticClient;
        private readonly IElasticSearchRepository _elasticRepository;
        private readonly IKafkaProducer _kafkaProducer;


        public ModifyPermissionRepository(AppDbContext context, PermissionMapper mapper, IElasticClient elasticClient,
            IElasticSearchRepository elasticSearchRepository, IKafkaProducer kafkaProducer )
        { 
            _context = context;
            _mapper = mapper;
            _elasticClient = elasticClient;
            _elasticRepository = elasticSearchRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<PermissionDTO> ModifyPermission(int id, PermissionRequest request)
        {
            try
            {
                var existingPermission = await _context.Permissions.FindAsync(id);
                var permissionType = await _context.PermissionTypes.FirstOrDefaultAsync(p => p.Descripcion == request.PermissionType);

                if (existingPermission != null && permissionType != null)
                {
                    UpdatePermissionDetails(existingPermission, request, permissionType);
                    _context.Permissions.Update(existingPermission);
                    await _context.SaveChangesAsync();
                    Log.Information("Permiso modificado: {@id}", id);

                    var elasticPermission = await _elasticRepository.CreateElasticPermission(existingPermission, permissionType);
                    var indexResponse = await _elasticClient.IndexDocumentAsync(elasticPermission);
                    Log.Information("Permiso de elasticSearch: {@id}", elasticPermission);

                    var updatedPermissionDTO = _mapper.MapPermissionToDTO(existingPermission);
                    await SendToKafka("modify");
                    return updatedPermissionDTO;
                }
                else
                {
                    HandlePermissionUpdateFailure(existingPermission, permissionType);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al modificar el permiso");

                throw new Exception("Ocurrió un error durante la actualización del permiso.", ex);

            }

        }

        #region Metodos
        private void UpdatePermissionDetails(Permission existingPermission, PermissionRequest request, PermissionTypes permissionType)
        {
            existingPermission.NombreEmpleado = request.NombreEmpleado;
            existingPermission.ApellidoEmpleado = request.ApellidoEmpleado;
            existingPermission.FechaPermiso = DateTime.Now;
            existingPermission.TipoPermiso = permissionType.Id;

        }

        private void HandlePermissionUpdateFailure(Permission existingPermission, PermissionTypes permissionType)
        {
            if (existingPermission == null)
            {
                Log.Error( "No existe el permiso especificado");

                throw new ArgumentException("El permiso especificado no existe.");
            }

            if (permissionType == null)
            {
                Log.Error("El tipo de permiso especificado no existe.");

                throw new ArgumentException("El tipo de permiso especificado no existe.");
            }
        }


        private async Task SendToKafka(string operationType)
        {
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = operationType
            };

            await _kafkaProducer.ProduceMessageAsync("modify-topic", message);
        }

        #endregion 
    }
}

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
using System.Runtime.CompilerServices;

namespace N5Challenge.Infrastructure.Repositories.RequestPermission
{
    public class RequestPermissionRepository : IRequestPermissionRepository
    {

        private readonly AppDbContext _context;
        private readonly PermissionMapper _mapper;
        private readonly IElasticClient _elasticClient;
        private readonly IElasticSearchRepository _elasticRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public RequestPermissionRepository(AppDbContext context, PermissionMapper mapper, IElasticClient elasticClient,
            IElasticSearchRepository elasticSearchRepository, IKafkaProducer kafkaProducer)

        {
            _context = context;
            _mapper = mapper;
            _elasticClient = elasticClient;
            _elasticRepository = elasticSearchRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<PermissionDTO> ProcessRequestPermission(PermissionRequest request)
        {
            try
            {
                var permissionType = await GetPermissionType(request.PermissionType);

                if (permissionType != null)
                {
                    var permission = await CreatePermissionAndSaveToContext(request, permissionType);
                    Log.Information("Permiso modificado: {@id}", permission);
                    await HandlePostPermissionCreationTasks(permission, permissionType);
                    await SendToKafka("modify");
                    var createdPermissionDTO = _mapper.MapPermissionToDTO(permission);
                    return createdPermissionDTO;
                }
                else
                {
                    Log.Error("No se encontró el tipo de permiso");

                    throw new Exception("No se encontró el tipo de permiso");

                }
            }
            catch(Exception ex) 
            {
                Log.Error("No se puedo crear el permiso nuevo, pruebe nuevamente luego");

                throw new Exception("No se puedo crear el permiso nuevo, pruebe nuevamente luego", ex);
            }

        }


        #region Metodos

        private async Task<PermissionTypes> GetPermissionType(string type)
        {
            return await _context.PermissionTypes.FirstOrDefaultAsync(p => p.Descripcion == type);
        }

        private async Task HandlePostPermissionCreationTasks(Permission permission, PermissionTypes permissionType)
        {
            var elasticPermission = await _elasticRepository.CreateElasticPermission(permission, permissionType);
            await _elasticClient.IndexDocumentAsync(elasticPermission);
        }

        private async Task<Permission> CreatePermissionAndSaveToContext(PermissionRequest request, PermissionTypes permissionType)
        {
            var permission = new Permission
            {
                NombreEmpleado = request.NombreEmpleado,
                ApellidoEmpleado = request.ApellidoEmpleado,
                PermissionType = permissionType,
                FechaPermiso = DateTime.Now,
            };

            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return permission;
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

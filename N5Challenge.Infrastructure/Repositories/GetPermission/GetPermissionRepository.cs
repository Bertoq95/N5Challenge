using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Domain.Entities;
using N5Challenge.Infrastructure.EntityFramework;
using N5Challenge.Infrastructure.Kafka.Producer;
using N5Challenge.Infrastructure.Mapping;
using Nest;
using Serilog;

namespace N5Challenge.Infrastructure.Repositories.GetPermission
{
    public class GetPermissionRepository : IGetPermissionRepository
    {
        private readonly AppDbContext _context;
        private readonly PermissionMapper _mapper;
        private readonly IElasticClient _elasticClient;
        private readonly IKafkaProducer _kafkaProducer;


        public GetPermissionRepository(AppDbContext context, PermissionMapper mapper,IElasticClient elasticClient, IKafkaProducer kafkaProducer)
        {
            _context = context;
            _mapper = mapper;
            _elasticClient = elasticClient;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<List<PermissionDTO>> GetAllPermissions()
        {
            var permissionList =await _context.Permissions.ToListAsync();
            var getPermissionDTO = permissionList.Select(permission => _mapper.MapPermissionToDTO(permission)).ToList();
            await SendToKafka("modify");
            await ElasticsearchIndex(permissionList);
            return getPermissionDTO;

        }
        private async Task ElasticsearchIndex(List<Permission> permissions)
        {
            var elasticClient = _elasticClient;
            foreach (var permission in permissions)
            {
                var indexResponse = await elasticClient.IndexDocumentAsync(permission);

                if (!indexResponse.IsValid)
                {
                    Log.Error($"Error al indexar documento: {indexResponse.DebugInformation}");
                }
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
    }
}

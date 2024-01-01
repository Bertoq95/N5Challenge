using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5Challenge.Infrastructure.Kafka.Producer;
using N5Challenge.Infrastructure.Mapping;
using N5Challenge.Infrastructure.Repositories.ElasticSearch;
using N5Challenge.Infrastructure.Repositories.GetPermission;
using N5Challenge.Infrastructure.Repositories.ModifyPermission;
using N5Challenge.Infrastructure.Repositories.RequestPermission;
using Nest;

namespace N5Challenge.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IModifyPermissionRepository, ModifyPermissionRepository>();
            services.AddScoped<IRequestPermissionRepository, RequestPermissionRepository>();
            services.AddScoped<IGetPermissionRepository, GetPermissionRepository>();
            services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();



            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            return services;

        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<PermissionMapper>();
            services.AddScoped<IElasticClient>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var clusterUrl = config.GetSection("ElasticsearchConfig:ClusterUrl").Value;

                var settings = new ConnectionSettings(new Uri(clusterUrl)).DefaultIndex("defaultindex");
                return new ElasticClient(settings);
            });

            services.AddScoped<IKafkaProducer>(provider =>
             {
                 var config = provider.GetRequiredService<IConfiguration>();
                 var bootstrapServers = config.GetSection("KafkaConfig:BootstrapServers").Value;

                 var producerConfig = new ProducerConfig
                 {
                     BootstrapServers = bootstrapServers
                 };

                 return new KafkaProducer(producerConfig);
             });

            return services;
        }



    }
}

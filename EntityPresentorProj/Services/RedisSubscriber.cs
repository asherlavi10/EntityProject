using EntityDataContract;
using EntityPresentorProj.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace EntityPresentorProj.Services
{
    public class RedisSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IMangeEntityPointService _mangeEntityPointService;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisSubscriber(IConfiguration configuration , IMangeEntityPointService mangeEntityPointService , IConnectionMultiplexer connectionMultiplexer)
        {
            _configuration  =configuration;
            _mangeEntityPointService = mangeEntityPointService;
            _connectionMultiplexer = connectionMultiplexer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            var sub = _connectionMultiplexer.GetSubscriber();

            sub.Subscribe(Consts.RedisChanelForNewEntity, (channel, message) => {

                var obj = JsonSerializer.Deserialize<EntityMessage>(message);
                _mangeEntityPointService.CreateNewImage(obj);

            });
            
            await Task.Delay(Timeout.Infinite, stoppingToken);

        }
    }
}

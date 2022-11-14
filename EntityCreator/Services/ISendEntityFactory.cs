using EntityCreator.Sender;
using EntityDataContract;

namespace EntityCreator.Services
{
    public interface ISendEntityFactory
    {
        public void Send(EntityDto entityDto);
    }

    public class SendEntityFactory : ISendEntityFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRabbitSender _rabbitSender;
        public SendEntityFactory(IConfiguration configuration, IHttpClientFactory httpClientFactory, IRabbitSender rabbitSender)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _rabbitSender = rabbitSender;
        }
        public async void Send(EntityDto entityDto)
        {
            var res = _configuration.GetSection("UseRedisPubsub");
            //call  to entity presenation application
            if (res.Value != true.ToString())
            {
                _rabbitSender.PublishMessage<EntityDto>(entityDto, "entity.create");
            }
            else
            {
                var client = _httpClientFactory.CreateClient("EntityPresentor");
                await client.PostAsJsonAsync<EntityDataContract.EntityDto>("/Home/CreateNewMap", entityDto);
            }
        }
    }
}


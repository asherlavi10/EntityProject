using EntityDataContract;
using EntityPresentorProj.Hubs;
using EntityPresentorProj.Services;
using Messaging;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EntityPresentorProj
{
	public class RabbitReceiver : IHostedService
	{
		private readonly RabbitMqSettings _rabbitSettings;
		private readonly IModel _channel;
		private readonly IHubContext<ImageHub> _imageHub;
		private readonly IMangeEntityPoint _mangeEntityPoint;
        private readonly IWebHostEnvironment _hostEnvironment;
	
		private readonly IConfiguration _configuration;
        public RabbitReceiver(RabbitMqSettings rabbitSettings, IModel channel, IHubContext<ImageHub> hub, IMangeEntityPoint mangeEntityPoint, IWebHostEnvironment hostEnvironment , IConfiguration configuration)
        {
            _rabbitSettings = rabbitSettings;
            _channel = channel;
            _imageHub = hub;
            _mangeEntityPoint = mangeEntityPoint;
            _hostEnvironment = hostEnvironment;
			_configuration = configuration;
        }

        public override bool Equals(object? obj)
        {
            return obj is RabbitReceiver receiver &&
                   EqualityComparer<IWebHostEnvironment>.Default.Equals(_hostEnvironment, receiver._hostEnvironment);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_hostEnvironment);
        }

        public Task StartAsync(CancellationToken cancellationToken)
		{
			DoStuff();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_channel.Dispose();
			return Task.CompletedTask;
		}

		private void DoStuff()
		{
			_channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName,
				type: _rabbitSettings.ExchangeType);

			var queueName = _channel.QueueDeclare().QueueName;


			_channel.QueueBind(queue: queueName,
							  exchange: _rabbitSettings.ExchangeName,
							  routingKey: "entity.create");


			var consumerAsync = new AsyncEventingBasicConsumer(_channel);
			consumerAsync.Received += async (_, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var entity = JsonSerializer.Deserialize<EntityDataContract.EntityDto>(message);
				var imgNewGuid = $"/img/{Guid.NewGuid().ToString()}.gif";
				var newimage = $"/wwwroot{imgNewGuid}";

				var curImg = Consts.PrifixKey + entity.AppKey;
				_ = _mangeEntityPoint.GetCurImage(curImg)
								 .SetBasePath(_hostEnvironment.WebRootPath)
								  .DrawImage(entity, imgNewGuid)
								  .SetCurImage(curImg, imgNewGuid)
								 .PublishToClientImageAsync(newimage);

				await _imageHub.Clients.All.SendAsync("ReceiveMessage",DateTime.Now.ToString(), newimage);

				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume(queue: queueName,
								 autoAck: false,
								 consumer: consumerAsync);
		}
	}
}

using EntityDataContract;
using EntityPresentorProj.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace EntityPresentorProj.Services
{
    public class MangeEntityPoint : IMangeEntityPoint
    {
        private readonly ICacheService _cacheService;
        private readonly IDrawPpointService _drawPpointService;
        private string _currImage;
        private string _basewwwrootPath;
        private SignalROptions _signalROptions;
        public MangeEntityPoint(ICacheService cacheService, IDrawPpointService drawPpointService, IOptions<SignalROptions> signalROptions)
        {
            _cacheService = cacheService;
            _drawPpointService = drawPpointService;
            _signalROptions = signalROptions.Value;
        }

        public string CurImg { get =>  _currImage; }
        
        public MangeEntityPoint GetCurImage(string key)
        {
            _currImage = _cacheService.GetValueAsString(key);
            if (string.IsNullOrEmpty(_currImage))
            {
                _cacheService.SetStringValue(key, Consts.MainImage);
                _currImage = Consts.MainImage;
            }
            return this;

        }
        public MangeEntityPoint DrawImage(EntityDto entityDto, string newPath)
        {
            _drawPpointService.DrawEntity(entityDto, _basewwwrootPath +"\\" +_currImage, _basewwwrootPath+newPath);
            return this;
        }

        public MangeEntityPoint SetCurImage(string key, string value)
        {
            _cacheService.SetStringValue(key,value);
            _currImage = value;
            return this;
        }
        public async Task<MangeEntityPoint> PublishToClientImageAsync(string imgNewGuid)
        {
            HubConnection connection = new HubConnectionBuilder()
           .WithUrl(_signalROptions.HubUrl, options => {
               options.Transports = HttpTransportType.WebSockets;
           }).WithAutomaticReconnect().Build();

            var date = DateTime.Now.ToString();
            await connection.StartAsync();
            await connection.InvokeAsync("SendMessage", date, imgNewGuid);
            return this;
        }
        public MangeEntityPoint SetBasePath(string basewwwrootPath)
        {
            _basewwwrootPath = basewwwrootPath;
            return this;
        }
        
    }
}

using EntityDataContract;

using EntityPresentorProj.Hubs;
using EntityPresentorProj.Models;
using EntityPresentorProj.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Drawing;

namespace EntityPresentorProj.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDrawPpointService _drawPpointService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IDrawPpointService drawPpointService, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _drawPpointService = drawPpointService;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
        [HttpPost]
        public async Task<IActionResult> GetEntityAsync([FromBody] EntityDto entityDto)
        {
           
           var curImg = HttpContext.Session.GetString("curImg");
            if (string.IsNullOrEmpty(curImg))
            {
                HttpContext.Session.SetString("curImg", Consts.MainImage);
                curImg = Consts.MainImage ;
            }
            
            var basewwwrootPath = _hostEnvironment.WebRootPath;
            var imgNewGuid = $"/img/{Guid.NewGuid().ToString()}.gif";
            var entity = _drawPpointService.DrawEntity(entityDto, basewwwrootPath + curImg, basewwwrootPath + imgNewGuid);
            
            HttpContext.Session.SetString("curImg", imgNewGuid);
            // publish signalr 
            #region signalr

            var url = _configuration["SignalrUrl"];
            HubConnection connection = new HubConnectionBuilder()
           .WithUrl("http://localhost:5116/chatHub", options => {
               options.Transports = HttpTransportType.WebSockets;
           }).WithAutomaticReconnect().Build();

            var newimage = $"../wwwroot{imgNewGuid}";
            var date =DateTime.Now.ToString();
            await connection.StartAsync();
            await connection.InvokeAsync("SendMessage",date, newimage);
            
            #endregion

            return View(new ImageModel { Name = Consts.MainImage, UpdateDate = DateTime.Now });

            

        }
    }
}
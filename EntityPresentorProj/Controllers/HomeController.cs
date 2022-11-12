using EntityDataContract;

using EntityPresentorProj.Hubs;
using EntityPresentorProj.Models;
using EntityPresentorProj.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Diagnostics;
using System.Drawing;

namespace EntityPresentorProj.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMangeEntityPoint _mangeEntityPoint;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IMangeEntityPoint mangeEntityPoint, IConfiguration configuration)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _mangeEntityPoint = mangeEntityPoint;
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
        public async Task<IActionResult> CreateNewMap([FromBody] EntityDataContract.EntityDto entityDto)
        {

            if (ModelState.IsValid)
            {
            
                
                using ConnectionMultiplexer redis =   ConnectionMultiplexer.Connect(_configuration.GetSection("RedisCacheUrl").Value);
                var sub = redis.GetSubscriber();

                sub.Subscribe(Consts.RedisChanelForNewEntity, (channel, imgNewGuid) => {

                    var newimage = $"../wwwroot{imgNewGuid}";

                    var curImg = Consts.PrifixKey + entityDto.AppKey;
                    _ = _mangeEntityPoint.GetCurImage(curImg)
                                     .SetBasePath(_hostEnvironment.WebRootPath)
                                      .DrawImage(entityDto, imgNewGuid)
                                      .SetCurImage(curImg, imgNewGuid)
                                     .PublishToClientImageAsync(newimage);

                });

                //create a publisher
                var pub = redis.GetSubscriber();
                var count = pub.Publish(Consts.RedisChanelForNewEntity, $"/img/{Guid.NewGuid().ToString()}.gif");
                //ISubscriber sub = redis.GetSubscriber();
                
                
                

            }

            return View();

        }
    }
}
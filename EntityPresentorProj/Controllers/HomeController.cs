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
using System.Text.Json;

namespace EntityPresentorProj.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public HomeController(ILogger<HomeController> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connectionMultiplexer = connectionMultiplexer;
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
                
                //create a publisher
                var pub = _connectionMultiplexer.GetSubscriber();
                EntityMessage entityMessage = new EntityMessage { EntityDto = entityDto, NewImage = $"/img/{Guid.NewGuid().ToString()}.gif" };
                var obj=  JsonSerializer.Serialize<EntityMessage>(entityMessage);
                var count = pub.Publish(Consts.RedisChanelForNewEntity, obj);
                
            }

            return View();

        }
    }
}
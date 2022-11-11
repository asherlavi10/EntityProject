using EntityDataContract;

using EntityPresentorProj.Hubs;
using EntityPresentorProj.Models;
using EntityPresentorProj.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Drawing;

namespace EntityPresentorProj.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMangeEntityPoint _mangeEntityPoint;
        
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IMangeEntityPoint mangeEntityPoint)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _mangeEntityPoint = mangeEntityPoint;   
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
            
                var imgNewGuid = $"/img/{Guid.NewGuid().ToString()}.gif";
                var newimage = $"../wwwroot{imgNewGuid}";

                _ = _mangeEntityPoint.GetCurImage("curImg")
                                 .SetBasePath(_hostEnvironment.WebRootPath)
                                  .DrawImage(entityDto, imgNewGuid)
                                  .SetCurImage("curImg", imgNewGuid)
                                 .PublishToClientImageAsync(newimage);

            }

            return View();

        }
    }
}
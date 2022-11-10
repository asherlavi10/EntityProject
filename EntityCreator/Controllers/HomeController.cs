using AutoMapper;
using EntityDataContract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
 

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
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
        public async Task<IActionResult> SendAsync(EntityModel entity)
        {
            var entityDto = _mapper.Map<EntityDto>(entity);

            //call  to entity presenation application
            var client = _httpClientFactory.CreateClient("EntityPresentor");
            var res = await client.PostAsJsonAsync<EntityDto>("/Home/GetEntity", entityDto);
            entity.ReurnMessage = "Your entity was updated.";
            return View(entity);
        }

    }
}
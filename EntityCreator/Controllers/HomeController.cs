using AutoMapper;
using EntityCreator;
using EntityDataContract;
using FluentValidation;
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
        private readonly IValidator<EntityDataContract.EntityDto> _validator;
        private readonly IConfiguration _configuration;
        private readonly RabbitSender _rabbitSender;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IMapper mapper, IValidator<EntityDto> validator, IConfiguration configuration, RabbitSender rabbitSender)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _validator = validator;
            _configuration = configuration;
            _rabbitSender = rabbitSender;
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
            if (ModelState.IsValid)
            {
                
                var entityDto = _mapper.Map<EntityDataContract.EntityDto>(entity);
                entityDto.AppKey = _configuration.GetSection("AppEntityKey")?.Value;
                if (_validator.Validate(entityDto).IsValid)
                {
                    var res = _configuration.GetSection("UseRedisPubsub");
                    //call  to entity presenation application
                    if(res.Value != true.ToString())
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
            return View(entity);
        }

    }
}
using AutoMapper;
using EntityCreator;
using EntityCreator.Services;
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
        private readonly IMapper _mapper;
        private readonly IValidator<EntityDataContract.EntityDto> _validator;
        private readonly IConfiguration _configuration;
        
        private readonly ISendEntityFactory _sendEntityFactory;
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IValidator<EntityDto> validator, IConfiguration configuration, ISendEntityFactory sendEntityFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
            _configuration = configuration;
            _sendEntityFactory = sendEntityFactory;
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
                    _sendEntityFactory.Send(entityDto);

                }
                
            }
            return View(entity);
        }

    }
}
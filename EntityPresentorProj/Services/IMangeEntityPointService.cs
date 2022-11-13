using EntityDataContract;
using EntityPresentorProj.Models;

namespace EntityPresentorProj.Services
{
    public interface IMangeEntityPointService
    {
        public void CreateNewImage(EntityMessage entityDto);
    }
    public class MangeEntityPointService : IMangeEntityPointService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMangeEntityPoint _mangeEntityPoint;
        private readonly IConfiguration _configuration;
        public MangeEntityPointService(IMangeEntityPoint mangeEntityPoint, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment=hostEnvironment;
            _mangeEntityPoint= mangeEntityPoint;
            _configuration=configuration;
        }
        public void CreateNewImage(EntityMessage entity)
        {
            var imgNewGuid = entity.NewImage;
            var newimage = $"../wwwroot{imgNewGuid}";

            var curImg = Consts.PrifixKey + entity.EntityDto.AppKey;
            _ = _mangeEntityPoint.GetCurImage(curImg)
                            .SetBasePath(_hostEnvironment.WebRootPath)
                             .DrawImage(entity.EntityDto, imgNewGuid)
                             .SetCurImage(curImg, imgNewGuid)
                            .PublishToClientImageAsync(newimage);
        }
    }
}

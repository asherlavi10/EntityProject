using EntityDataContract;

namespace EntityPresentorProj.Services
{
    public interface IMangeEntityPoint
    {
          public Task<MangeEntityPoint> PublishToClientImageAsync(string imgNewGuid);
          public MangeEntityPoint SetCurImage( string key, string value);
          public MangeEntityPoint DrawImage(EntityDataContract.EntityDto entityDto, string newPath);
          public MangeEntityPoint GetCurImage(string key);
          public MangeEntityPoint SetBasePath(string basewwwrootPath);
    }
}

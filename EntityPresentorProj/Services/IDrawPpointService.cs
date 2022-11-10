using EntityDataContract;

namespace EntityPresentorProj.Services
{
    public interface IDrawPpointService
    {
        EntityDto DrawEntity(EntityDto entityDto, string imgPath, string newPath);

    }
}

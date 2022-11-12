using AutoMapper;
using EntityDataContract;
using WebApplication6.Models;

namespace EntityCreator.Profiles
{
    public class EntityProfile :Profile
    {
        public EntityProfile()
        {
            CreateMap<EntityDto, EntityModel>();
            CreateMap<EntityModel, EntityDto>().ForMember(x=>x.AppKey,y=>y.Ignore());

        }
    }
}

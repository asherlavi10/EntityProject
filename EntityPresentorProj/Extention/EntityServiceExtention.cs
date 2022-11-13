using EntityDataContract.Validor;
using EntityPresentorProj.Services;
using FluentValidation;

namespace EntityPresentorProj.Extention
{
    public static class EntityServiceExtention
    {
        public static IServiceCollection AddEntityServies(this IServiceCollection services)
        {
            services.AddTransient<IDrawPpointService, DrawPpointService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<IMangeEntityPoint, MangeEntityPoint>();
            services.AddTransient<IValidator<EntityDataContract.EntityDto>, EntityValidator>();
            services.AddTransient<IMangeEntityPointService, MangeEntityPointService>();
            return services;
        }
    }
}

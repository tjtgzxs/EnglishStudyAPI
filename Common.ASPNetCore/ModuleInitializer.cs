using Common.Commons;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ASPNetCOre;

public class ModuleInitializer:IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();
        services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();
    }
}
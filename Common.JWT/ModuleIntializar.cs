using Common.Commons;
using Microsoft.Extensions.DependencyInjection;

namespace Common.JWT;

public class ModuleInitializer:IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
    }
}
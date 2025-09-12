using Microsoft.Extensions.DependencyInjection;

namespace Common.Commons;

public interface IModuleInitializer
{
    public void Initialize(IServiceCollection services);
}
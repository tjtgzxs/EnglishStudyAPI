using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Commons;

public static class ModuleInitializerExtensions
{
    public static IServiceCollection RunModuleInitializers(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();
            var moduleInitializerTypes = types.Where(t => !t.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(t));
            foreach(var implType in moduleInitializerTypes)
            {
                var initializer = (IModuleInitializer?)Activator.CreateInstance(implType);
                if (initializer == null)
                {
                    throw new ApplicationException($"Cannot create ${implType}");
                }
                initializer.Initialize(services);
            }
            
        }

        return services;
    }
}
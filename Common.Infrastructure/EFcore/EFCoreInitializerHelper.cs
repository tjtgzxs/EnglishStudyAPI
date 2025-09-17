using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure;

public static class EFCoreInitializerHelper
{
    public static IServiceCollection AddEFCoreInitializer(this IServiceCollection services,
        Action<DbContextOptionsBuilder> builder,
        IEnumerable<Assembly> assemblies)
    {
        Type[] types = new Type[] { typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime), typeof(ServiceLifetime) };
        var methodAddDbContext = typeof(EntityFrameworkServiceCollectionExtensions)
            .GetMethod(nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext), 1, types);
        foreach (var asmToLoad in assemblies)
        {
            Type[] typesInAsm = asmToLoad.GetTypes();
            //Register DbContext
            //GetTypes() include public/protected ones
            //GetExportedTypes only include public ones
            //so that XXDbContext in Agrregation can be internal to keep insulated
            foreach (var dbCtxType in typesInAsm
                         .Where(t => !t.IsAbstract && typeof(DbContext).IsAssignableFrom(t)))
            {
                //similar to serviceCollection.AddDbContextPool<ECDictDbContext>(opt=>new DbContextOptionsBuilder(dbCtxOpt));
                var methodGenericAddDbContext = methodAddDbContext.MakeGenericMethod(dbCtxType);
                methodGenericAddDbContext.Invoke(null, new object[] { services, builder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
            }
        }
        return services;
    }
    
}
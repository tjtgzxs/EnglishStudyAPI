using Common.EventBus;
using Microsoft.AspNetCore.Builder;

namespace CommonInitializer
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCommonInitializer(this IApplicationBuilder app)
        {
            app.UseEventBus();
            app.UseCors();
            app.UseForwardedHeaders();
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    
    }
}


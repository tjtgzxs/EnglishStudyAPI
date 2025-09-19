using Common.ASPNetCOre;
using Common.Commons;
using Common.Commons.JsonConverters;
using Common.EventBus;
using Common.Infrastructure;
using Common.JWT;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;

using JWTOptions = Common.JWT.JWTOptions;

namespace CommonInitializer;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureDbConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.ConfigureAppConfiguration((hostCtx, configBuilder) =>
        {
            string connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
            configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr), reloadOnChange: true,
                reloadInterval: TimeSpan.FromSeconds(5));
        });
    }

    public static void ConfiureExtraServices(this WebApplicationBuilder builder, InitializerOptions initOptions)
    {
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;
        var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
        services.RModuleInitializers(assemblies);
        services.AddEFCoreInitializer(ctx =>
        {
            string connStr = configuration.GetValue<string>("DefaultDB:ConnStr");
            ctx.UseMySql(connStr,ServerVersion.AutoDetect(connStr));
        },assemblies);
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();
        JWTOptions jwtOptions = configuration.GetSection("JWT").Get<JWTOptions>();
        builder.Services.AddJWTAuthentication(jwtOptions);
        builder.Services.Configure<SwaggerGenOptions>(c =>
        {
            c.AddSwagAuthenticationHeader();
        });
        services.AddMediatR(c =>
        {
            foreach (var assembly in assemblies)
            {
                c.RegisterServicesFromAssembly(assembly);
            }
            
        });
        services.Configure<MvcOptions>(o =>
        {
            o.Filters.Add<UnitOfWorkFilter>();
        });
        services.Configure<JsonOptions>(o =>
        {
            o.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
        });
        services.AddCors(o =>
        {
            var corOpt = configuration.GetSection("Cors").Get<CorsSettings>();
            string[] urls = corOpt.Origins;
            o.AddDefaultPolicy(builder=>builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        });
        services.AddLogging(builder =>
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console()
                .WriteTo.File(initOptions.LogFilePath)
                .CreateLogger();
            builder.AddSerilog();
        });
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblies(assemblies);
        });
        services.Configure<JWTOptions>(configuration.GetSection("JWT"));
        services.Configure<IntegrationEventRabbitMQOptions>(configuration.GetSection("RabbitMQ"));
        services.AddMoreEventBus(initOptions.EventBusQueueName, assemblies);
        string redisConnStr = configuration.GetValue<string>("Redis:ConnStr");
        IConnectionMultiplexer redisConnMultiplexer= ConnectionMultiplexer.Connect(redisConnStr);
        services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
        });
    }

}
using System.ComponentModel.DataAnnotations;
using CommonSharedLibrary.DependencyInjection.Authenticatio;
using CommonSharedLibrary.Middleware.ApiGatewayListener;
using CommonSharedLibrary.Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace CommonSharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services,
            IConfiguration config, string fileName) where TContext : DbContext
        {
            //Add Generic DB Context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("eCommerceConnection"),
                sqlServerOption => sqlServerOption.EnableRetryOnFailure()));
            //Configure Serilog logging
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Debug().WriteTo.Console().WriteTo
                .File(path:$"{fileName}-.text",
                    restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,
                    outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zz} [{Level:u3}] {message:lj} {NewLine} {Exception}",
                    rollingInterval:RollingInterval.Day).CreateLogger();
            //Add JWT Authentication scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //Use global exception
            app.UseMiddleware<GlobalExceptionHandler>();
            //Register middleware to block all outsider API calls
            app.UseMiddleware<ApiGatewayListener>();
            return app;
        }
    }
}


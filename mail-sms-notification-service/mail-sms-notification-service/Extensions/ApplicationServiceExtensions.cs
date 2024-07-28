using mail_sms_notification_service.configuration;
using mail_sms_notification_service.Filters;
using mail_sms_notification_service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SecurePayEmailService.Extension;

public static class ApplicationServicerExtensions
{
    private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SecurityRequirementsOperationFilter>();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sid Digital Mailing and Sms Services.",
                Version = "v1",
                Description = "This is the directory of the Sid Digital APIs for Mailing and Sms Services",
                Contact = new OpenApiContact
                {
                    Name = "Mailing and Sms Services.",
                    Url = new Uri("https://sidd.com")
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Description = "Place to add JWT with Bearer",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name="Bearer",
                    },
                    new List<string>()
                }
            });
        });
        return services;
    }


    public static WebApplication AddApplicationBuilder(WebApplication app)
    {
        app.UseResponseCaching();
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("Cache-control", "no-store");
            context.Response.Headers.Add("Pragma", "no-cache");
            context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade");
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000;includeSubDomains;");
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
            context.Response.Headers.Add("Content-Security-Policy", "unsafe-inline 'self'");
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none';");
            await next();
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
     
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        var smtpSettings = services.Configure<SmtpConfiguration>(configuration.GetSection("SmtpConfigurationSettings"));
        services.AddSingleton(smtpSettings);
        services.AddTransient<INotificationService, NotificationService>();
        services.AddMvc(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        });
        services.ConfigureSwagger();
        return services;
    }
}
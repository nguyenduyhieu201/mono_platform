using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mono.BusinessService.Filters;
using Mono.BusinessService.Interfaces;
using Mono.BusinessService.Services;
using Mono.CoreService.Filters;
using Mono.CoreService.Interfaces;
using Mono.CoreService.Services;
using Mono.Repository.Data;
using Mono.SharedLibrary.Mappings;
using Mono.SharedLibrary.Messages;
using Mono.SharedLibrary.Models;
using RabbitMQ.Client;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using System.Text;

namespace Mono.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureLoggerService(this IServiceCollection services, ILoggingBuilder logging, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .CreateLogger();
            logging.ClearProviders();
            logging.AddSerilog(logger);

            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(
                opts => opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly(typeof(RepositoryContext).Assembly.FullName)));

            //// Config DbContextPooling 
            //services.AddDbContextPool<RepositoryContext>(
            //    opts => opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
            //        b => b.MigrationsAssembly(typeof(RepositoryContext).Assembly.FullName)));
        }


        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<ICoreServiceManager, CoreServiceManager>();
            services.AddScoped<IBusinessServiceManager, BusinessServiceManager>();
        }

        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<TeacherMappingProfile>();
                map.AddProfile<StudentMappingProfile>();
                map.AddProfile<UserMappingProfile>();
                map.AddProfile<DocumentMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }


        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("30SecondsCaching", new CacheProfile
                {
                    Duration = 30
                });
            });
        }
        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Student Teacher API",
                    Version = "v1",
                    Description = "Student Teacher API Services.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ajide Habeeb."
                    },
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(configuration["RabbitMQSettings:HostAddress"]);

                    cfg.Message<SendNotificationEvent>(e => e.SetEntityName("mono-exchange")); // name of the primary exchange
                    cfg.Publish<SendNotificationEvent>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
                    cfg.Send<SendNotificationEvent>(e =>
                    {
                        e.UseRoutingKeyFormatter(context => context.Message.provider.ToString()); // route by provider (email or fax)
                    });
                });
            });
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<AttributeValidation>();
            services.AddScoped<TeacherExistsValidation>();
            services.AddScoped<StudentExistsValidation>();
        }
    }

}

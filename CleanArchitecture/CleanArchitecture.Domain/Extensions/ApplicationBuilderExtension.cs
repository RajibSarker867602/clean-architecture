using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.SwaggerConfigurations;
using CleanArchitecture.Domain.SignalR;

namespace CleanArchitecture.Domain.Extensions
{
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// Enable application CORS policies
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppCORSPolicies(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "all", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                    //policy.WithOrigins(new string[] { "http://localhost:4200/", "" });
                });
            });
            return builder;
        }

        /// <summary>
        /// Add application authenticaiton with Jwt Authentication
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(c =>
            {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                c.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return builder;
        }

        /// <summary>
        /// Application swagger configurations
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppSwaggerGen(this WebApplicationBuilder builder)
        {
            // congigurations
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOption>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-Token`"
                });
                options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
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

                // write document comments for swagger
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                foreach (string fileName in xmlFiles)
                {
                    string xmlFilePath = Path.Combine(AppContext.BaseDirectory, fileName);
                    if (File.Exists(xmlFilePath))
                        options.IncludeXmlComments(xmlFilePath, includeControllerXmlComments: true);
                }

                options.OperationFilter<SwaggerDefaultValue>();
            });

            return builder;
        }

        /// <summary>
        /// Application signalR configurations
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppSignalR(this WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR().AddHubOptions<MessageHub>(options =>
            {
                options.EnableDetailedErrors = true;
                options.EnableDetailedErrors = true;
                //options.KeepAliveInterval = TimeSpan.FromMinutes(15);
                //options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);

                options.KeepAliveInterval = TimeSpan.FromSeconds(30);
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
            }).AddJsonProtocol(options =>
            {
                //options.PayloadSerializerSettings.ContractResolver =
                //      new DefaultContractResolver();
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            return builder;
        }

        /// <summary>
        /// Configure application newtonsoftjson with filters
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppAddNewtonsoftJson(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(configure =>
            {
                configure.ReturnHttpNotAcceptable = true;
                configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                configure.Filters.Add(new ConsumesAttribute("Application/json"));   // for input request accept header
                configure.Filters.Add(new ProducesAttribute("Application/json"));   // for output response accept header
            }).AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).AddXmlDataContractSerializerFormatters();
            builder.Services.Configure<FormOptions>(x =>
            {
                x.ValueCountLimit = int.MaxValue;
                x.ValueLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = Int32.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
            });

            return builder;
        }

        /// <summary>
        /// Configure application session
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppSession(this WebApplicationBuilder builder)
        {
            builder.Services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromHours(1);
                options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(builder.Configuration.GetSection("Jwt:Access_Token_ExpireTime_In_Minutes").Value));
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return builder;
        }

        public static WebApplicationBuilder AddAppRouteGuard(this WebApplicationBuilder builder)
        {
            return builder;
        }

        /// <summary>
        /// Register static appsettings contents
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddStaticAppSettingsContents(this WebApplicationBuilder builder)
        {
            //ServerConfigurations.APIServerBaseUrl = builder.Configuration["ApplicationSettings:APIServerBaseUrl"];
            //ServerConfigurations.ServerControllerURL = builder.Configuration["ApplicationSettings:ServerControllerURL"];

            return builder;
        }
    }
}

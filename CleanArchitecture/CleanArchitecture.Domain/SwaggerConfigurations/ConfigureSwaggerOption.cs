using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.SwaggerConfigurations
{
    /// <summary>
    /// Swagger configuration
    /// </summary>
    public class ConfigureSwaggerOption : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOption(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "Clean Architecture API",
                Version = description.ApiVersion.ToString(),
                Description = "Through this API documentations, you'll get the API list of Clean Architecture API",
                Contact = new()
                {
                    Name = "Rajib Sarker",
                    Email = "rajibsarker320@gmail.com",
                    Url = new Uri("https://rajibsarker320.blogspot.com/")
                },
            };

            if (description.IsDeprecated)
            {
                info.Description = "This API version is deprecated.";
            }

            return info;
        }
    }
}

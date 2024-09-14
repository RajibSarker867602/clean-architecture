using CleanArchitecture.Application;
using CleanArchitecture.Domain.Extensions;
using CleanArchitecture.Domain.Filters;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Presentation;
using CleanArchitecture.Repositories;
using CleanArchitecture.WebApi.ExceptionHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// sesstion configurations
builder.AddAppSession();
// for caching
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// static appsettings contents
builder.AddStaticAppSettingsContents();

// NewtonsoftJson configurations
builder.AddAppAddNewtonsoftJson();
builder.Services.AddMvc(option =>
{
    option.MaxModelBindingCollectionSize = int.MaxValue;
});

// signalR configurations
builder.AddAppSignalR();

// model validation attribute reginstration
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        // model validation filter register here
        options.Filters.Add(typeof(ValidateModelFilter));
    }).AddApplicationPart(CleanArchitecture.Presentation.DependencyInjection.Assembly);

// swagger authorizations
builder.Services.AddEndpointsApiExplorer();
builder.AddAppSwaggerGen();

//CORS Policy
builder.AddAppCORSPolicies();

// Jwt token authentication enable
builder.AddAppAuthentication();

#region Route Gurd

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

#endregion Route Gurd

//TODO: mapster
//TODO: fluent validation
//TODO: serilog


// services registration
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation()
    .AddRepositories();

// global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

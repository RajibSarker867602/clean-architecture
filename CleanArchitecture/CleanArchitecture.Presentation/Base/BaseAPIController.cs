using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Presentation.Base;

[ApiController]
public abstract class BaseAPIController : ControllerBase
{
    private readonly ISender _sender;

    protected ISender Sender => _sender
        ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}

using CleanArchitecture.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstractions.Messaging;

/// <summary>
/// Command
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Command
/// </summary>
/// <typeparam name="TResponse">Response Type</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
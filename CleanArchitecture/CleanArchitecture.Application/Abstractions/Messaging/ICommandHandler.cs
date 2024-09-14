using CleanArchitecture.Domain.Abstractions.Messaging;
using CleanArchitecture.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging;

/// <summary>
/// Command handler
/// </summary>
/// <typeparam name="TCommand">Command Type</typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary>
/// Command handler
/// </summary>
/// <typeparam name="TCommand">Command Type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
{
}
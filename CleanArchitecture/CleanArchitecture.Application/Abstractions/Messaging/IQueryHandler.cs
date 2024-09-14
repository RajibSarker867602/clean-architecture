using CleanArchitecture.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging;

/// <summary>
/// Query handler
/// </summary>
/// <typeparam name="TQuery">Query type</typeparam>
/// <typeparam name="TResponse">Query response</typeparam>
public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
{
}
using CleanArchitecture.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging;

/// <summary>
/// Query
/// </summary>
/// <typeparam name="TResponse">Query response</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
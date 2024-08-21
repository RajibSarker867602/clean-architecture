using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Department.Command;
public sealed class CreateDepartmentCommandHandler : ICommandHandler<CreateDepartmentCommand>
{
    public async Task<Result> Handle(CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        if (request is null) return DepartmentErrors.NotFound;

        return Result.Success(request);
    }
}

public sealed class DepartmentErrors
{
    public static readonly Error NotFound = new("Departments.NotFound", "No data found!");
}
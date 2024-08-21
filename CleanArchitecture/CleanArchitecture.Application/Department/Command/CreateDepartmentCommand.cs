using CleanArchitecture.Domain.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Department.Command
{
    public sealed record CreateDepartmentCommand(long Id, string Name) : ICommand;
}

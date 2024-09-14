using CleanArchitecture.Domain.Abstractions.Messaging;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.ToDo.Command
{
    public sealed record CreateToDoItemCommand(string ToDoName, bool IsClosed) : ICommand<ToDoItem>;
}

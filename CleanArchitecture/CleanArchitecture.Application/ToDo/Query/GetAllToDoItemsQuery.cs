using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.ToDo.Query
{
    public sealed record GetAllToDoItemsQuery : IQuery<ICollection<ToDoItem>>;
}

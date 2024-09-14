using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Repositories.Abstraction.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.ToDo.Query
{
    public sealed class GetAllToDoItemQueryHandler(IToDoItemRepository repository)
        : IQueryHandler<GetAllToDoItemsQuery, ICollection<ToDoItem>>
    {
        public async Task<Result<ICollection<ToDoItem>>> Handle(GetAllToDoItemsQuery request, CancellationToken cancellationToken)
        {
            return Result.Success<ICollection<ToDoItem>>(await repository.GetAllAsync(cancellationToken));
        }
    }
}

using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Repositories.Abstraction.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.ToDo.Command
{
    public sealed class CreateToDoItemCommandHandler(IToDoItemRepository repository) : ICommandHandler<CreateToDoItemCommand, ToDoItem>
    {
        public async Task<Result<ToDoItem>> Handle(CreateToDoItemCommand request, CancellationToken cancellationToken)
        {
            var created = await repository.AddAsync(new ToDoItem() { ItemName = request.ToDoName, IsClosed = request.IsClosed }, cancellationToken);
            if (created is null || created.IsFailure) return Result.Failure<ToDoItem>(created.Error);

            return Result.Success<ToDoItem>(created.Value);
        }
    }
}

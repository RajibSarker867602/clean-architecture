using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Repositories.Abstraction.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Repositories.Abstraction.Repos
{
    public interface IToDoItemRepository : IRepository<ToDoItem>
    {
    }
}

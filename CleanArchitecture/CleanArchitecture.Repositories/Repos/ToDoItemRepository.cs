using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Repositories.Abstraction.Base;
using CleanArchitecture.Repositories.Abstraction.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Repositories.Repos
{
    internal class ToDoItemRepository : Repository<ToDoItem>, IToDoItemRepository
    {
        private readonly ApplicationDbContext _db;

        public ToDoItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

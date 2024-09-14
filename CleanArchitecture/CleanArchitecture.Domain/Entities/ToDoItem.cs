using CleanArchitecture.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities
{
    [Table("ToDoItems")]
    public class ToDoItem : Entity
    {
        public string ItemName { get; set; }
        public bool IsClosed { get; set; }
    }
}

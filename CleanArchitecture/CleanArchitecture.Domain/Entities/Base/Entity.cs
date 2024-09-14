using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Base
{
    public abstract class Entity
    {
        public long Id { get; set; }
    }
}

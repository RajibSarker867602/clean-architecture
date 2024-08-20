using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Utilities.Contracts
{
    public interface ICacheService
    {
        T GetData<T>(string key);

        bool SetData<T>(string key, T value, DateTimeOffset expires);

        object RemoveData(string key);
    }
}

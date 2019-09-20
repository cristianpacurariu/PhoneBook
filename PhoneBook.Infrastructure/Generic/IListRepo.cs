using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Infrastructure.Generic
{
    public interface IListRepo<T>
    {
        List<T> All();
    }
}

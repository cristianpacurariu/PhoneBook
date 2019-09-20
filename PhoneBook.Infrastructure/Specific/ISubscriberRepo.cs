using PhoneBook.Infrastructure.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Infrastructure.Specific
{
    public interface ISubscriberRepo<T,F> : IAddRepo<T>, IDeleteRepo<T>, IFilterRepo<T,F>, IGetRepo<T>,
                                              IListRepo<T>, IUpdateRepo<T>
    {
    }
}

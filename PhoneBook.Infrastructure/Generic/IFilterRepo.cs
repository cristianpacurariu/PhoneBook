using System.Collections.Generic;

namespace PhoneBook.Infrastructure.Generic
{
    public interface IFilterRepo<T,F>
    {
        List<T> Filter(F filter);
    }
}

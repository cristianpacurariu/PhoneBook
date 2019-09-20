using System.Collections.Generic;

namespace PhoneBook.Infrastructure.Generic
{
    public interface IListRepo<T>
    {
        List<T> All();
    }
}

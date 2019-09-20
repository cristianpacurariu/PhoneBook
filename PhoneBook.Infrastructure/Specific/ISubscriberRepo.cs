using PhoneBook.Infrastructure.Generic;

namespace PhoneBook.Infrastructure.Specific
{
    public interface ISubscriberRepo<T,F> : IAddRepo<T>, IDeleteRepo<T>, IFilterRepo<T,F>, IGetRepo<T>,
                                              IListRepo<T>, IUpdateRepo<T>
    {
    }
}

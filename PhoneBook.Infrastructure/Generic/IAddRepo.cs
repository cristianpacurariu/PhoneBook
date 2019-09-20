namespace PhoneBook.Infrastructure.Generic
{
    public interface IAddRepo<T>
    {
        void Add(T item);
    }
}

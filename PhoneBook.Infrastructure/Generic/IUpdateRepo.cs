namespace PhoneBook.Infrastructure.Generic
{
    public interface IUpdateRepo<T>
    {
        void Update(T item);
    }
}

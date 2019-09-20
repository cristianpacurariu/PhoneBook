namespace PhoneBook.Infrastructure.Generic
{
    public interface IGetRepo<T>
    {
        T Get(int id);
    }
}

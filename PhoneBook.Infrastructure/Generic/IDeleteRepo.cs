namespace PhoneBook.Infrastructure.Generic
{
    public interface IDeleteRepo<T>
    {
        bool Delete(int id);
    }
}

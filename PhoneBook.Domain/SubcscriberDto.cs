using System;

namespace PhoneBook.Domain
{
    [Serializable]
    public class SubscriberDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Details { get; set; }
    }
}

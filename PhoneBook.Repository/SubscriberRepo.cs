using PhoneBook.DataAcces.Model;
using PhoneBook.Domain;
using PhoneBook.Infrastructure.Specific;
using System.Collections.Generic;
using System.Linq;

namespace PhoneBook.Repository
{
    public class SubscriberRepo : ISubscriberRepo<SubscriberDto, SubscriberFilterDto>
    {
        public void Add(SubscriberDto item)
        {
            using (GoldenPagesEntities context = new GoldenPagesEntities())
            {
                Subscriber toAdd = new Subscriber
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    PhoneNumber = item.PhoneNumber,
                    Details = item.Details
                };

                context.Subscribers.Add(toAdd);
                context.SaveChanges();
            }
        }
        public List<SubscriberDto> All()
        {
            using (GoldenPagesEntities context = new GoldenPagesEntities())
            {
                List<SubscriberDto> allSubscribers = new List<SubscriberDto>();
                List<Subscriber> fromDb = context.Subscribers.ToList();

                foreach (Subscriber subscriber in fromDb)
                {
                    SubscriberDto toAddToList = new SubscriberDto
                    {
                        Id = subscriber.Id,
                        FirstName = subscriber.FirstName,
                        LastName = subscriber.LastName,
                        PhoneNumber = subscriber.PhoneNumber,
                        Details = subscriber.Details
                    };
                    allSubscribers.Add(toAddToList);
                }
                return allSubscribers;
            }
        }
        public bool Delete(int id)
        {
            using (GoldenPagesEntities context = new GoldenPagesEntities())
            {
                Subscriber toDelete = context.Subscribers.SingleOrDefault(d => d.Id == id);

                if (toDelete == null)
                {
                    return false;
                }

                context.Subscribers.Remove(toDelete);
                context.SaveChanges();
                return true;
            }
        }
        public List<SubscriberDto> Filter(SubscriberFilterDto filter)
        {
            using (GoldenPagesEntities context = new GoldenPagesEntities())
            {
                List<SubscriberDto> filtered = new List<SubscriberDto>();

                foreach (Subscriber subscriber in context.Subscribers.ToList())
                {
                    if (subscriber.FirstName.ToUpper().Contains(filter.textToSearchFor.ToUpper()) || 
                        subscriber.LastName.ToUpper().Contains(filter.textToSearchFor.ToUpper()) || 
                        subscriber.PhoneNumber.ToUpper().Contains(filter.textToSearchFor.ToUpper()) || 
                        subscriber.Details.ToUpper().Contains(filter.textToSearchFor.ToUpper()))
                    {
                        SubscriberDto dto = new SubscriberDto
                        {
                            Id = subscriber.Id,
                            FirstName = subscriber.FirstName,
                            LastName = subscriber.LastName,
                            PhoneNumber = subscriber.PhoneNumber,
                            Details = subscriber.Details
                        };
                        filtered.Add(dto);
                    }
                }
                
                return filtered;
            }
        }
        public SubscriberDto Get(int id)
        {
            using (GoldenPagesEntities context = new GoldenPagesEntities())
            {
                Subscriber fromDb = context.Subscribers.SingleOrDefault(d => d.Id == id);
                if (fromDb == null)
                {
                    return null;
                }

                SubscriberDto toReturn = new SubscriberDto
                {
                    Id = fromDb.Id,
                    FirstName = fromDb.FirstName,
                    LastName = fromDb.LastName,
                    PhoneNumber = fromDb.PhoneNumber,
                    Details = fromDb.Details
                };
                return toReturn;
            }
        }
        public void Update(SubscriberDto entity)
        {
            using (GoldenPagesEntities context = new GoldenPagesEntities())
            {
                Subscriber toUpdate = context.Subscribers.SingleOrDefault(d => d.Id == entity.Id);
                toUpdate.FirstName = entity.FirstName;
                toUpdate.LastName = entity.LastName;
                toUpdate.PhoneNumber = entity.PhoneNumber;
                toUpdate.Details = entity.Details;

                context.Subscribers.Attach(toUpdate);
                context.Entry(toUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}

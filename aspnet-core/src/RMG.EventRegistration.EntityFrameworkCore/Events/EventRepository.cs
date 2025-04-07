using Microsoft.EntityFrameworkCore;
using RMG.EventRegistration.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace RMG.EventRegistration.Events
{
    public class EventRepository : EfCoreRepository<EventRegistrationDbContext, Event, Guid>, IEventRepository
    {
        public EventRepository(IDbContextProvider<EventRegistrationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<IQueryable<Event>> GetQueryableAsNoTracking(bool includeRegistrations)
        {
            var queryable = await GetQueryableAsync();

            if (includeRegistrations) 
            {
                return (await GetQueryableAsync())
                    .Include(e => e.Registrations)
                    .AsNoTracking();
            }

            return (await GetQueryableAsync())
                          .AsNoTracking();
        }

        public async Task<IQueryable<Event>> GetQueryableIncludeUsers()
        {
            return (await GetQueryableAsync())
                    .Include(e => e.Registrations)
                        .ThenInclude(r => r.User)
                    .AsNoTracking();
        }
    }
}

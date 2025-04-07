using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace RMG.EventRegistration.Events
{
    public interface IEventRepository : IRepository<Event, Guid>
    {
        Task<IQueryable<Event>> GetQueryableAsNoTracking(bool includeRegistrations);
        Task<IQueryable<Event>> GetQueryableIncludeUsers();
    }
}

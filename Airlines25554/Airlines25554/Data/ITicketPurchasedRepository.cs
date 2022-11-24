using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface ITicketPurchasedRepository : IGenericRepository<TicketPurchased>
    {
        List<TicketPurchased> BoughtTicketsByUser(User user);

       
        public IQueryable TicketListByFlight(Flight flight);

    }
}

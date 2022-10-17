using Airlines25554.Data.Entities;
using System.Collections.Generic;

namespace Airlines25554.Data
{
    public interface ITicketPurchasedRepository : IGenericRepository<TicketPurchased>
    {
        List<TicketPurchased> BoughtTicketsByUser(User user);


    }
}

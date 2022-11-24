using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Esf;
using System.Collections.Generic;
using System.Linq;

namespace Airlines25554.Data
{
    public class TicketPurchasedRepository : GenericRepository<TicketPurchased>, ITicketPurchasedRepository
    {
        private readonly DataContext _context;

        public TicketPurchasedRepository(DataContext context) : base(context)   
        {
            _context = context;
        }


        public List<TicketPurchased> BoughtTicketsByUser(User user)
        {
            return _context.PurchasedTickets
                .Include(c => c.Flight)
                .ThenInclude(c => c.From)
                .Include(c => c.Flight)
                .ThenInclude(c => c.To)
                .Include(c => c.User)
                .Where(x => x.User == user)
                .ToList();
        }

        public IQueryable TicketListByFlight(Flight flight)
        {
            return _context.PurchasedTickets.Where(f => f.Flight == flight);
        }
    }
}

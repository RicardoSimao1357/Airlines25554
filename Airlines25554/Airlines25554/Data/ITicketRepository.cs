using Airlines25554.Data.Entities;
using System.Collections.Generic;

namespace Airlines25554.Data
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        List<Ticket> FlightTickets(int flightId);

        List<Ticket> FlightTicketsByUser(string email);
    }
}

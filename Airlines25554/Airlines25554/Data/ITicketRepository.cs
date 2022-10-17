using Airlines25554.Data.Entities;
using Airlines25554.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airlines25554.Data
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        List<Ticket> FlightTickets(int flightId);

        List<Ticket> FlightTicketsByUser(string email);


        public Ticket GetTicketById(int ticketId);


        public void  UpdateTicketIsAvailableAsync(Ticket ticket);





    }
}

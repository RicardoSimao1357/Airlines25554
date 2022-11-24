using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines25554.Data
{

    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private DataContext _context { get; set; }

        private Random _random;

        public TicketRepository(
            DataContext context) : base(context)
        {
            _context = context;
            _random = new Random(); 
        }

        public List<Ticket> FlightTickets(int flightId, string ticketClass)
        {
            return _context.Tickets
                .Include(c => c.Flight)
                .Where(x => x.Flight.Id == flightId && x.Class == ticketClass)
                .ToList();
        }

        public List<Ticket> AvailableFlightTickets(int flightId)
        {
            return _context.Tickets
                .Include(c => c.Flight)
     
                .Where(x => x.Flight.Id == flightId && x.IsAvailable == true)
          
                .ToList();
        }

        public List<Ticket> FlightTicketsByUser(string email)
        {
            return _context.Tickets
                .Include(c => c.Flight)
                .ThenInclude(c => c.From)
                .Include(c => c.Flight)
                .ThenInclude(c => c.To)
                .Include(c => c.User)
                .Where(x => x.User.Email == email)
                .ToList();
        }

        public void UpdateTicketIsAvailable(Ticket ticket)
        {
        

            ticket.IsAvailable = false;
            _context.Tickets.Update(ticket);
            _context.SaveChangesAsync();
          
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _context.Tickets.Where(t => t.Id == ticketId).FirstOrDefault();  
        }
    }
}
    


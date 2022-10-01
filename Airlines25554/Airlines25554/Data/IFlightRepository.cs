using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Airlines25554.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
     
        IEnumerable<SelectListItem> GetComboAirplanes();

        IEnumerable<SelectListItem> GetComboAirports();

        Task<Airport> GetAirportAsync(int id);

        List<Flight> GetAllWithObjects();

        Task<Flight> GetFlightWithObjectsAsync(int id);

        IEnumerable<SelectListItem> GetComboStatus();

        IEnumerable<Flight> GetFlightsByStatus(int statusId);

        void UpdateFlightStatus(DateTime date);

        bool AirplaneIsAvailable(int id, DateTime departure, DateTime arrival);

        List<Ticket> GetTickets(int flightId);








    }
}

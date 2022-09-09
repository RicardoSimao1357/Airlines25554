using Airlines25554.Data.Entities;
using Airlines25554.Models;

namespace Airlines25554.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public AirPlane ToAirPlane(AirPlaneViewModel model, string path, bool isNew)
        {
            return new AirPlane
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                AirplaneModel = model.AirplaneModel,
                Registration = model.Registration,
                EconomySeats = model.EconomySeats,
                ExecutiveSeats = model.ExecutiveSeats,
                FirstClassSeats = model.FirstClassSeats,
                User = model.User
            };
        }

        public AirPlaneViewModel ToAirPlaneViewModel(AirPlane airPlane)
        {
             return new AirPlaneViewModel
            {
                Id = airPlane.Id,
                AirplaneModel = airPlane.AirplaneModel,
                EconomySeats = airPlane.EconomySeats,
                ExecutiveSeats = airPlane.ExecutiveSeats,
                FirstClassSeats = airPlane.FirstClassSeats,
                ImageUrl = airPlane.ImageUrl,
                User = airPlane.User

            };
        }
    }
}

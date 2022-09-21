using Airlines25554.Data.Entities;
using Airlines25554.Models;
using System;

namespace Airlines25554.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public AirPlane ToAirPlane(AirPlaneViewModel model, Guid imageId, bool isNew)
        {
            return new AirPlane
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId ,
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
                ImageId = airPlane.ImageId,
                User = airPlane.User

            };
        }

        public Customer ToCustomer(CustomerViewModel model, Guid imageId, bool isNew)
        {
            return new Customer
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,    
                LastName = model.LastName,
                Address = model.Address,
                PassportId = model.PassportId,
                ImageId = imageId,
                User = model.User
            };
        }

        public CustomerViewModel ToCustomerViewModel(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,  
                LastName = customer.LastName,    
                Address = customer.Address, 
                PassportId = customer.PassportId,   
                ImageId = customer.ImageId,
                User = customer.User    

            };
        }
    }
}

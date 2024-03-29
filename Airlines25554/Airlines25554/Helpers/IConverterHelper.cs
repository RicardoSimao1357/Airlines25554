﻿using Airlines25554.Data.Entities;
using Airlines25554.Models;
using System;

namespace Airlines25554.Helpers
{
    public interface IConverterHelper
    {
        AirPlane ToAirPlane(AirPlaneViewModel model, Guid imageId, bool isNew);

        AirPlaneViewModel ToAirPlaneViewModel(AirPlane airPlane);

        Customer ToCustomer(CustomerViewModel model, Guid imageId, bool isNew);

        CustomerViewModel ToCustomerViewModel(Customer customer);

        Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew);

        EmployeeViewModel ToEmployeeViewModel(Employee employee);


        Country ToCountry(CountryViewModel model, Guid imageId, bool isNew);

        CountryViewModel ToCountryViewModel(Country country);
    }
}

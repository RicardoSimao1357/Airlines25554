using Airlines25554.Data.Entities;
using Airlines25554.Models;
using System;

namespace Airlines25554.Helpers
{
    public interface IConverterHelper
    {
        AirPlane ToAirPlane(AirPlaneViewModel model, Guid imageId, bool isNew);

        AirPlaneViewModel ToAirPlaneViewModel(AirPlane airPlane);
    }
}

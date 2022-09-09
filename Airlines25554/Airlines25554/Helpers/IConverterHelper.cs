using Airlines25554.Data.Entities;
using Airlines25554.Models;

namespace Airlines25554.Helpers
{
    public interface IConverterHelper
    {
        AirPlane ToAirPlane(AirPlaneViewModel model, string path, bool isNew);

        AirPlaneViewModel ToAirPlaneViewModel(AirPlane airPlane);
    }
}

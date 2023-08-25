using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiXTelematics.Assesment.Models
{
    class VehiclePosition
    {
        public int VehicleId { get; }
        public string VehicleRegistration { get; }
        public Coordinate Coordinate { get; }
        public DateTime RecordedTimeUTC { get; }

        public VehiclePosition(int vehicleId, string vehicleRegistration, double latitude, double longitude, DateTime recordedTimeUTC)
        {
            VehicleId = vehicleId;
            VehicleRegistration = vehicleRegistration;
            Coordinate = new Coordinate(latitude, longitude);
            RecordedTimeUTC = recordedTimeUTC;
        }
    }
}

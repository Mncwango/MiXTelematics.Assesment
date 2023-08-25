using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiXTelematics.Assesment.Models
{
    internal class Coordinate
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }


        public double DistanceTo(Coordinate other)
        {
            // Haversine formula
            double earthRadius = 6371000; // Earth's radius in meters

            double lat1Rad = Latitude * Math.PI / 180;
            double lon1Rad = Longitude * Math.PI / 180;
            double lat2Rad = other.Latitude * Math.PI / 180;
            double lon2Rad = other.Longitude * Math.PI / 180;

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadius * c;
        }
    }
}

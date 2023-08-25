using MiXTelematics.Assesment.Models;
using System.Reflection.PortableExecutable;
using System.Text;

namespace MiXTelematics.Assesment
{
    internal class Program
    {
        private const string FILE_NAME = "VehiclePositions.dat"; 
        static List<VehiclePosition> LoadVehiclePositions(string filePath)
        {
            List<VehiclePosition> vehiclePositions = new List<VehiclePosition>();
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        int vehicleId = reader.ReadInt32();

                        StringBuilder registrationBuilder = new StringBuilder();
                        char c;
                        while ((c = reader.ReadChar()) != 0)
                        {
                            registrationBuilder.Append(c);
                        }
                        string vehicleRegistration = registrationBuilder.ToString();

                        float latitude = reader.ReadSingle();
                        float longitude = reader.ReadSingle();
                        ulong recordedTimeUTCSeconds = reader.ReadUInt64();

                        DateTime recordedTimeUTC = UnixTimeStampToDateTime(recordedTimeUTCSeconds);

                        vehiclePositions.Add(new VehiclePosition(vehicleId, vehicleRegistration, latitude, longitude, recordedTimeUTC));
                    }
                }

               
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error loading vehicle positions: {ex.Message}");
            }

            return vehiclePositions;
        }

        static List<VehiclePosition> FindNearestVehiclePositions(string filePath, List<Coordinate> coordinates)
        {
            List<VehiclePosition> vehiclePositions = LoadVehiclePositions(filePath);
            if (vehiclePositions.Count == 0)
            {
                Console.WriteLine("No vehicle positions loaded. Exiting...");
                return new List<VehiclePosition>();
            }
            VehicleLookup kdTree = new VehicleLookup(vehiclePositions);

            List<VehiclePosition> nearestPositions = new List<VehiclePosition>();

            foreach (var coordinate in coordinates)
            {
                var nearestPosition = kdTree.FindNearestNeighbor(coordinate);
                nearestPositions.Add( nearestPosition);
            }

            return nearestPositions;
        }

        static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixEpoch.AddSeconds(unixTimeStamp);
        }
        static void Main(string[] args)
        {
            List<Coordinate> coordinates = new List<Coordinate>
        {
            new Coordinate(34.544909, -102.100843),
            new Coordinate(32.345544, -99.123124),
            new Coordinate(33.234235, -100.214124),
            new Coordinate(35.195739, -95.348899),
            new Coordinate(31.895839, -97.789573),
            new Coordinate(32.895839, -101.789573),
            new Coordinate(34.115839, -100.225732),
            new Coordinate(32.335839, -99.992232),
            new Coordinate(33.535339, -94.792232),
            new Coordinate(32.234235, -100.222222)
        };

            List<VehiclePosition> nearestPositions =  FindNearestVehiclePositions(FILE_NAME, coordinates);

            foreach (var position in nearestPositions)
            {
                Console.WriteLine($"Coordinate: Lat={position.Coordinate.Latitude}, Long={position.Coordinate.Longitude}");
                Console.WriteLine($"Nearest Vehicle Registration: {position.VehicleRegistration}");
                Console.WriteLine("--------------------------------------------------");
            }

            Console.WriteLine("Nearest vehicle positions found.");

            Console.ReadLine();
        }
    }
   

   

  


  



}
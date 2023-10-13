namespace Velozient.DroneDeliveryService.Models
{
    public class Location
    {
        public string Name { get; }
        public int PackageWeight { get; }
        public bool CheckDeliverie { get; set; }
        public Location(string name, int weight)
        {
            Name = name;
            PackageWeight = weight;
        }

        public static List<Location> CreateLocations(string dirPath)
        {
            var locationsList = File.ReadAllLines(dirPath);
            locationsList = locationsList.Skip(1).ToArray();
            locationsList = UseFulString.RemoveBrackets(locationsList);

            var locations = locationsList.Select(s => s.Split(", ")).Select(a => new Location(a[0],
                Convert.ToInt32(a[1]))).ToList();
            return locations;
        }

        public static bool CheckDeliveries(List<Location> locations)
        {
            return locations.Exists(c => c.CheckDeliverie == false);
        }
    }
}

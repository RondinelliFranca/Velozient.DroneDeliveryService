namespace Velozient.DroneDeliveryService.Models
{
    public class Delivery
    {
        public string Name { get; }
        public int PackageWeight { get; }
        public bool TriedLoad { get; set; }

        public Delivery(string name, int weight)
        {
            Name = name;
            PackageWeight = weight;
        }

        public static List<Delivery> CreateDeliveries(string dirPath)
        {
            var locationsList = File.ReadAllLines(dirPath);
            locationsList = locationsList.Skip(1).ToArray();
            locationsList = UsefulString.RemoveBrackets(locationsList);

            var locations = locationsList.Select(deliveryData => deliveryData.Split(", ")).Select(a =>
            {
                var name = a[0];
                var weight = Convert.ToInt32(a[1]);

                return new Delivery(name, weight);
            }).ToList();

            return locations;
        }

        public static bool CheckDeliveries(List<Delivery> locations)
        {
            return locations.Exists(c => c.TriedLoad == false);
        }
    }
}

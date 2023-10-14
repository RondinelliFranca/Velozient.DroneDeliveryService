namespace Velozient.DroneDeliveryService.Models
{
    public class Drone
    {
        public string Name { get; }
        public int MaximumCapacity { get; }
        public int CurrentWeight { get; set; }
        public List<Delivery>? Deliveries { get; set; }
        public int Trip { get; set; }
        public List<Trip> Trips { get; set; }

        public Drone(string name, int maxCapacity)
        {
            Name = name;
            MaximumCapacity = maxCapacity;
            Deliveries = new List<Delivery>();
            Trips = new List<Trip>();
        }

        public static List<Drone> CreateDrones(string dirPath)
        {
            var dronesString = File.ReadLines(dirPath).First().Split(", ");
            dronesString = UsefulString.RemoveBrackets(dronesString);
            var drones = new List<Drone>();
            for (var i = 0; i < dronesString.Length; i += 2)
            {
                var name = dronesString[i];
                var maximumCapacity = Convert.ToInt32(dronesString[i + 1]);
                drones.Add(new Drone(name, maximumCapacity));
            }

            return drones;
        }

        public bool CheckIfDroneHasCapacity(int packageWeight)
        {
            return CurrentWeight + packageWeight <= MaximumCapacity;
        }
        public void ReturnHomeBase()
        {
            CurrentWeight = 0;
            Deliveries = null;
            Deliveries = new List<Delivery>();
        }
        public void AddDelivery(Delivery delivery)
        {
            delivery.TriedLoad = true;
            CurrentWeight += delivery.PackageWeight;
            Deliveries.Add(delivery);
        }

        public bool IsFullLoad() => CurrentWeight == MaximumCapacity;
        public bool HasSomeDelivery() => Deliveries.Any();

        public bool IsTheWeightSupported(int packageWeight) => MaximumCapacity >= packageWeight;
    }
}


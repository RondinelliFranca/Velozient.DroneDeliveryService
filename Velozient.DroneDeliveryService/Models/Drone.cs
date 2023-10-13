namespace Velozient.DroneDeliveryService.Models
{
    public class Drone
    {
        public string Name { get; }
        public int MaximumCapacity { get; }
        public int CurrentWeight { get; set; } = 0;
        public List<Location>? Deliveries { get; set; }
        public int Trip { get; set; } = 0;
        public List<Trip> Trips { get; set; }

        public Drone(string name, int maxCapacity)
        {
            Name = name;
            MaximumCapacity = maxCapacity;
            Deliveries = new List<Location>();
            Trips = new List<Trip>();
        }

        public static List<Drone> CreateDrones(string dirPath)
        {
            var dronesString = File.ReadLines(dirPath).First().Split(", ");
            dronesString = UseFulString.RemoveBrackets(dronesString);
            var drones = new List<Drone>();
            for (var i = 0; i < dronesString.Length; i += 2)
            {
                var name = dronesString[i];
                var maximumCapacity = Convert.ToInt32(dronesString[i + 1]);
                drones.Add(new Drone(name, maximumCapacity));
            }

            return drones;
        }

        public static bool CheckDroneFullLoad(List<Drone> drones)
        {
            return drones.Exists(e => e.CurrentWeight != e.MaximumCapacity);
        }

        public static bool CheckCurrentDroneCapacity(int currentWeightDrone, int maximumCapacity, int packageWeight)
        {
            return currentWeightDrone + packageWeight > maximumCapacity;
        }
        public static void ReturnToHomeBase(Drone drone)
        {
            drone.CurrentWeight = 0;
            drone.Deliveries = null;
            drone.Deliveries = new List<Location>();
        }

        public static bool CheckPossibleDeliver(List<Drone> drones, int packageWeight)
        {
            return drones.Exists(d => d.MaximumCapacity >= packageWeight);
        }
        public static void ToDoTrip(List<Drone> drones)
        {
            foreach (var drone in drones.Where(drone => drone.Deliveries.Any()).OrderBy(d => d.Name))
            {
                drone.Trip++;
                drone.Trips.Add(new Trip(drone.Trip, drone.Deliveries));

                Drone.ReturnToHomeBase(drone);
            }
        }
    }
}

using System.Text;
using Velozient.DroneDeliveryService.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter or past Path:");
        var dirPath = Console.ReadLine();
        

        if (File.Exists(dirPath))
        {
            var drones = Drone.CreateDrones(dirPath);

            var locations = Location.CreateLocations(dirPath);

            // Sort drones in descending order of max weight
            drones = drones.OrderByDescending(d => d.MaximumCapacity).ToList();

            // Sort locations in descending order of package weight
            locations = locations.OrderByDescending(l => l.PackageWeight).ToList();

            //Create output file
            using var sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(dirPath), "Output.txt"));

            OtimizeTrips(locations, drones, sw);
            Trip.GetTrips(drones, sw);
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    /// <summary>
    /// Distribute locations to drones and optimize trips
    /// </summary>
    /// <param name="locations"></param>
    /// <param name="drones"></param>
    /// <param name="sw"></param>
    private static void OtimizeTrips(List<Location> locations, List<Drone> drones, StreamWriter sw)
    {
        foreach (var location in locations.Where(location => !location.CheckDeliverie))
        {
            foreach (var drone in drones)
            {
                if (!Drone.CheckDroneFullLoad(drones))
                {
                    Drone.ToDoTrip(drones);
                    OtimizeTrips(locations, drones, sw);
                    break;
                }
                if (drone.MaximumCapacity >= location.PackageWeight && drone.CurrentWeight <= drone.MaximumCapacity)
                {
                    if (!Drone.CheckCurrentDroneCapacity(drone.CurrentWeight, drone.MaximumCapacity, location.PackageWeight))
                    {
                        drone.CurrentWeight += location.PackageWeight;
                        drone.Deliveries.Add(location);
                        location.CheckDeliverie = true;
                        break;
                    }
                }
                else
                {
                    //If there is a weight that the drones cannot support
                    if (Drone.CheckPossibleDeliver(drones, location.PackageWeight)) continue;
                    sw.WriteLine($"This package: {location.Name} cannot be delivered, because its weight {location.PackageWeight} cannot be supported.");
                    location.CheckDeliverie = true;
                    sw.WriteLine(Environment.NewLine);
                    break;
                }
            }
        }
        while (Location.CheckDeliveries(locations))
        {
            Drone.ToDoTrip(drones);
            OtimizeTrips(locations, drones, sw);
        }
        Drone.ToDoTrip(drones);//the last trip
        
    }
}


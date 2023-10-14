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

            var deliveries = Delivery.CreateDeliveries(dirPath);

            // Sort drones in descending order of max weight
            drones = drones.OrderByDescending(d => d.MaximumCapacity).ToList();

            // Sort deliveries in descending order of package weight
            deliveries = deliveries.OrderByDescending(l => l.PackageWeight).ToList();

            //Create output file
            using var sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(dirPath), "Output.txt"));

            OptimizeTrips(deliveries, drones, sw);
            Trip.GetTrips(drones, sw);
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    /// <summary>
    /// Distribute deliveries to drones and optimize trips
    /// </summary>
    /// <param name="deliveries"></param>
    /// <param name="drones"></param>
    /// <param name="sw"></param>
    private static void OptimizeTrips(List<Delivery> deliveries, List<Drone> drones, StreamWriter sw)
    {
        foreach (var delivery in deliveries.Where(delivery => !delivery.TriedLoad))
        {
            foreach (var drone in drones)
            {
                if (drones.All(drone => drone.IsFullLoad()))
                {
                    Trip.ToDoTrip(drones);
                    OptimizeTrips(deliveries, drones, sw);
                    break;
                }

                if (drone.CheckIfDroneHasCapacity(delivery.PackageWeight))
                {
                    drone.AddDelivery(delivery);
                    break;
                }
                //If there is a weight that the drones cannot support
                if (drones.Any(drone => drone.IsTheWeightSupported(delivery.PackageWeight))) continue;
                sw.WriteLine(
                    $"This package: {delivery.Name} cannot be delivered, because its weight {delivery.PackageWeight} cannot be supported.");
                delivery.TriedLoad = true;
                sw.WriteLine(Environment.NewLine);
                break;
            }
        }

        while (Delivery.CheckDeliveries(deliveries))
        {
            Trip.ToDoTrip(drones);
            OptimizeTrips(deliveries, drones, sw);
        }
        Trip.ToDoTrip(drones); //the last trip
    } 
}
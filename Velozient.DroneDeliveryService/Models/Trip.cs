namespace Velozient.DroneDeliveryService.Models;

public class Trip
{
    public int NumberTrip { get; set; }
    public List<Delivery> Locations { get; set; }
    public Trip(int numberTrip, List<Delivery> locations)
    {
        NumberTrip = numberTrip;
        Locations = locations;
    }

    public static void GetTrips(List<Drone> drones, StreamWriter sw)
    {
        foreach (var drone in drones.OrderBy(d => d.Name))
        {
            sw.WriteLine($"[{drone.Name}]");

            foreach (var droneTrip in drone.Trips.OrderBy(t => t.NumberTrip))
            {
                sw.WriteLine($"Trip #{droneTrip.NumberTrip}");
                foreach (var location in droneTrip.Locations)
                {
                    sw.Write(location.Equals(droneTrip.Locations.Last()) ? $"[{location.Name}]" : $"[{location.Name}], ");
                }
                sw.WriteLine();
            }
            sw.WriteLine(Environment.NewLine);
        }
    }

    public static void ToDoTrip(List<Drone> drones)
    {
        foreach (var drone in drones.Where(drone => drone.HasSomeDelivery()).OrderBy(d => d.Name))
        {
            drone.Trip++;
            drone.Trips.Add(new Trip(drone.Trip, drone.Deliveries));

            drone.ReturnHomeBase();
        }
    }
}
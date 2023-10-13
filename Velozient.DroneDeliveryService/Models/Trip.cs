namespace Velozient.DroneDeliveryService.Models;

public class Trip
{
    public Trip(int numberTrip, List<Location> locations)
    {
        NumberTrip = numberTrip;
        Locations = locations;
    }
    public int NumberTrip { get; set; }
    public List<Location> Locations { get; set; }

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
}
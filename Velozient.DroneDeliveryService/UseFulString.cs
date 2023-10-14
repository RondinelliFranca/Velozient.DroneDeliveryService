namespace Velozient.DroneDeliveryService;

public static class UsefulString
{
    public static string[] RemoveBrackets(string[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = list[i].Replace("[", "").Replace("]", "");
        }
        return list;
    }
}
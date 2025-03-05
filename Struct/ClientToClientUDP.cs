using Newtonsoft.Json.Linq;
using Struct;

/// <summary>
/// Тест udp потока
/// </summary>
public class ClientToClientUDP : StructData
{
    public int Port;
    public ClientToClientUDP(int port)
    {
        Port = port;
    }
    public override JObject ToJObject()
    {
        JObject data = new JObject();
        data["type"] = "udp";
        data["port"] = Port;
        return data;
    }
    public static ClientToClientUDP? Convert(JObject jObject)
    {
        if (IsType(jObject, "udp"))
        {
            int port = GetPort(jObject);

            if (0 <= port)
            {
                return new ClientToClientUDP(port);
            }
        }
        return null;
    }
}

using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от сервера к клиенту с информацией о том, к какому пользователю нужно подключиться
    /// </summary>
    public class ServerToClientConnect : StructData
    {
        /// <summary>
        /// Имя пользователя который хочет общаться с клиентом
        /// </summary>
        public string Name;
        /// <summary>
        /// Порт пользователя который хочет общаться
        /// </summary>
        public int Port;
        /// <summary>
        /// IP адресс пользователя который хочет общаться
        /// </summary>
        public string IP;
        /// <summary>
        /// Ключ для верефикации у пользователя
        /// </summary>
        public string Key;
        public ServerToClientConnect(string name, int port, string iP, string key)
        {
            Name = name;
            Port = port;
            IP = iP;
            Key = key;
        }
        public override JObject ToJObject()
        {
            JObject data = new JObject();
            data["type"] = "connect";
            data["name"] = Name;
            data["port"] = Port;
            data["ip"] = IP;
            data["key"] = Key;
            return data;
        }
        public static ServerToClientConnect? Convert(JObject jObject)
        {
            if (IsType(jObject, "connect"))
            {
                string? name = GetString(jObject, "name");
                int port = GetPort(jObject);
                string? ip = GetString(jObject, "ip");
                string? key = GetString(jObject, "key");
                if (name != null && ip != null && key != null && 0 <= port)
                {
                    return new ServerToClientConnect(name, port, ip, key);
                }
            }
            return null;
        }
    }
}

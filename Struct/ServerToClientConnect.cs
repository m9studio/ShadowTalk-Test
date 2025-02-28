using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от сервера к клиенту с информацией о том, к какому пользователю нужно подключиться
    /// </summary>
    public class ServerToClientConnect
    {
        /// <summary>
        /// Имя пользователя который хочет общаться с клиентом
        /// </summary>
        public string Name;
        /// <summary>
        /// Порт пользователя который хочет общаться
        /// </summary>
        public ushort Port;
        /// <summary>
        /// IP адресс пользователя который хочет общаться
        /// </summary>
        public string IP;
        /// <summary>
        /// Ключ для верефикации у пользователя
        /// </summary>
        public string Key;
        public ServerToClientConnect(string name, ushort port, string iP, string key)
        {
            Name = name;
            Port = port;
            IP = iP;
            Key = key;
        }
        public override string ToString()
        {
            JObject data = new JObject();
            data["type"] = "connect";
            data["name"] = Name;
            data["port"] = Port;
            data["ip"] = IP;
            data["key"] = Key;
            return data.ToString();
        }
        public static ServerToClientConnect? Convert(JObject jObject)
        {
            if (IsType(jObject))
            {
                string? name = jObject.GetString("name");
                ushort port = 0;
                string? ip = jObject.GetString("ip");
                string? key = jObject.GetString("key");
                if (name != null && ip != null && key != null && ushort.TryParse((string?)jObject["port"], out port))
                {
                    return new ServerToClientConnect(name, port, ip, key);
                }
            }
            return null;
        }
        public static bool IsType(JObject jObject) => jObject.IsType("connect");
    }
}

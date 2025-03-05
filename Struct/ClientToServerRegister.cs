using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от клиента к серверу, с информацией для регистрации
    /// </summary>
    public class ClientToServerRegister : StructData
    {
        /// <summary>
        /// Наше имя
        /// </summary>
        public string Name;
        /// <summary>
        /// Порт для входящих подключений
        /// </summary>
        public int Port;
        public ClientToServerRegister(string name, int port)
        {
            Name = name;
            Port = port;
        }
        public override JObject ToJObject()
        {
            JObject data = new JObject();
            data["type"] = "register";
            data["name"] = Name;
            data["port"] = Port;
            return data;
        }
        public static ClientToServerRegister? Convert(JObject jObject)
        {
            if (IsType(jObject, "register"))
            {
                string? name = GetString(jObject, "name");
                int port = GetPort(jObject);

                if (name != null && 0 <= port)
                {
                    return new ClientToServerRegister(name, port);
                }
            }
            return null;
        }
    }
}

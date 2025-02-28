using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от клиента к серверу, с информацией для регистрации
    /// </summary>
    public class ClientToServerRegister
    {
        /// <summary>
        /// Наше имя
        /// </summary>
        public string Name;
        /// <summary>
        /// Порт для входящих подключений
        /// </summary>
        public ushort Port;
        public ClientToServerRegister(string name, ushort port)
        {
            Name = name;
            Port = port;
        }
        public override string ToString()
        {
            JObject data = new JObject();
            data["type"] = "register";
            data["name"] = Name;
            data["port"] = Port;
            return data.ToString();
        }
        public static ClientToServerRegister? Convert(JObject jObject)
        {
            if (IsType(jObject))
            {
                string? name = jObject.GetString("name");
                ushort port = 0;

                if (name != null && ushort.TryParse((string?)jObject["port"], out port))
                {
                    return new ClientToServerRegister(name, port);
                }
            }
            return null;
        }
        public static bool IsType(JObject jObject) => jObject.IsType("register");
    }
}

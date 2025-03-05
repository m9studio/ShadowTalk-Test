using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от клиента к клиенту с информацией о том, что мы подключились на его проcьбу
    /// </summary>
    public class ClientToClientConnect : StructData
    {
        /// <summary>
        /// Наше имя, чтобы другой клиент понял кто мы
        /// </summary>
        public string Name;
        /// <summary>
        /// Ключ для верефикации, чтобы другой клиент нас распознал
        /// </summary>
        public string Key;
        public ClientToClientConnect(string name, string key)
        {
            Name = name;
            Key = key;
        }
        public override JObject ToJObject()
        {
            JObject data = new JObject();
            data["type"] = "connect";
            data["name"] = Name;
            data["key"] = Key;
            return data;
        }
        public static ClientToClientConnect? Convert(JObject jObject)
        {
            if (IsType(jObject, "connect"))
            {
                string? name = GetString(jObject, "name");
                string? key = GetString(jObject, "key");
                if (name != null && key != null)
                {
                    return new ClientToClientConnect(name, key);
                }
            }
            return null;
        }
    }
}

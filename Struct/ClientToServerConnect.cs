using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от клиента к серверу, с просьбой чтобы к нам подключился пользователь
    /// </summary>
    public class ClientToServerConnect : StructData
    {
        /// <summary>
        /// Имя пользователя, с кем клиент хочет общаться
        /// </summary>
        public string Name;
        /// <summary>
        /// Ключ для верефикации, чтобы знать что подключается к нам именно запрашиваемый пользователь
        /// </summary>
        public string Key; //TODO может лучше ключ будет генерировать сервер?
        public ClientToServerConnect(string name, string key)
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
        public static ClientToServerConnect? Convert(JObject jObject)
        {
            if (IsType(jObject, "connect"))
            {
                string? name = GetString(jObject, "name");
                string? key = GetString(jObject, "key");
                if (name != null && key != null)
                {
                    return new ClientToServerConnect(name, key);
                }
            }
            return null;
        }
    }
}

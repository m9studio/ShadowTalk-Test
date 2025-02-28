using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от клиента к серверу, с просьбой чтобы к нам подключился пользователь
    /// </summary>
    public class ClientToServerConnect
    {
        /// <summary>
        /// Имя пользователя, с кем клиент хочет общаться
        /// </summary>
        public string Name;
        /// <summary>
        /// Ключ для верефикации, чтобы знать что подключается к нам именно запрашиваемый пользователь
        /// </summary>
        public string Key; //TODO может лучше ключ будет генерировать сервер?
        public static ClientToServerConnect? Convert(JObject jObject)
        {
            if (IsType(jObject))
            {
                string? name = jObject.GetString("name");
                string? key = jObject.GetString("key");
                if (name != null && key != null)
                {
                    ClientToServerConnect item = new ClientToServerConnect();
                    item.Name = name;
                    item.Key = key;
                    return item;
                }
            }
            return null;
        }
        public static bool IsType(JObject jObject) => jObject.IsType("connect");
    }
}

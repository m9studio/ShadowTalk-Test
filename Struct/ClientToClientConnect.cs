﻿using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Сообщение от клиента к клиенту с информацией о том, что мы подключились на его проcьбу
    /// </summary>
    public class ClientToClientConnect
    {
        /// <summary>
        /// Наше имя, чтобы другой клиент понял кто мы
        /// </summary>
        public string Name;
        /// <summary>
        /// Ключ для верефикации, чтобы другой клиент нас распознал
        /// </summary>
        public string Key;
        public static ClientToClientConnect? Convert(JObject jObject)
        {
            if (IsType(jObject))
            {
                string? name = jObject.GetString("name");
                string? key = jObject.GetString("key");
                if (name != null && key != null)
                {
                    ClientToClientConnect item = new ClientToClientConnect();
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

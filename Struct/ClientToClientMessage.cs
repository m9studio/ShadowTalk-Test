using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Обычное сообщение от клиента к клиенту
    /// </summary>
    public class ClientToClientMessage
    {
        public string Text;
        public static ClientToClientMessage? Convert(JObject jObject)
        {
            if (IsType(jObject))
            {
                string? text = jObject.GetString("text");
                if (text != null)
                {
                    ClientToClientMessage item = new ClientToClientMessage();
                    item.Text = text;
                    return item;
                }
            }
            return null;
        }
        public static bool IsType(JObject jObject) => jObject.IsType("message");
    }
}

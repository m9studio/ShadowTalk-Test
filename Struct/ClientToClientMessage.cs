using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Обычное сообщение от клиента к клиенту
    /// </summary>
    public class ClientToClientMessage
    {
        public string Text;
        public ClientToClientMessage(string text)
        {
            Text = text;
        }
        public override string ToString()
        {
            JObject data = new JObject();
            data["type"] = "message";
            data["text"] = Text;
            return data.ToString();
        }
        public static ClientToClientMessage? Convert(JObject jObject)
        {
            if (IsType(jObject))
            {
                string? text = jObject.GetString("text");
                if (text != null)
                {
                    return new ClientToClientMessage(text);
                }
            }
            return null;
        }
        public static bool IsType(JObject jObject) => jObject.IsType("message");
    }
}

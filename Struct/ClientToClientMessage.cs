using Newtonsoft.Json.Linq;

namespace Struct
{
    /// <summary>
    /// Обычное сообщение от клиента к клиенту
    /// </summary>
    public class ClientToClientMessage : StructData
    {
        public string Text;
        public ClientToClientMessage(string text)
        {
            Text = text;
        }
        public override JObject ToJObject()
        {
            JObject data = new JObject();
            data["type"] = "message";
            data["text"] = Text;
            return data;
        }
        public static ClientToClientMessage? Convert(JObject jObject)
        {
            if (IsType(jObject, "message"))
            {
                string? text = GetString(jObject, "text");
                if (text != null)
                {
                    return new ClientToClientMessage(text);
                }
            }
            return null;
        }
    }
}

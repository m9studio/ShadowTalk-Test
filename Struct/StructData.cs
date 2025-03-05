using Newtonsoft.Json.Linq;

public abstract class StructData
{
    public override string ToString() => ToJObject().ToString();
    public abstract JObject ToJObject();


    public static bool IsType(JObject jObject, string type) => GetType(jObject) == type;
    public static string? GetType(JObject jObject) => GetString(jObject, "type");
    public static string? GetString(JObject jObject, string key)
    {
        JToken? token = jObject.GetValue(key);
        if (token != null)
        {
            return token.ToString();
        }
        return null;
    }
    public static int? GetInt(JObject jObject, string key)
    {
        JToken? token = jObject.GetValue(key);
        if (token != null)
        {
            if (token.Type == JTokenType.Integer)
            {
                return (int?)token;
            }
            else if (token.Type == JTokenType.String)
            {
                int value;
                if (int.TryParse(token.ToString(), out value))
                {
                    return value;
                }
            }
        }
        return null;
    }
    public static int GetPort(JObject jObject)
    {
        int? value = GetInt(jObject, "port");
        if(value != null && 0 <= value && value <= ushort.MaxValue)
        {
            return (int)value;
        }
        return -1;
    }
}

using Newtonsoft.Json.Linq;

class Logger
{
    public virtual void Log(string text)
    {
        Console.WriteLine(text);
    }
    public void Log(JObject jObject)
    {
        Log(jObject.ToString());
    }
    public void Log(byte[] bytes)
    {
        Log(BitConverter.ToString(bytes).Replace("-", ""));
    }
}

using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;

public static class Core
{



    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }

    public static string GetMessage(this Socket socket)
    {
        //TODO переработать, т.к. если клиент не успеет вовремя прочесть сообщение (не забываем про разные задержки), то 2 и более сообщений сольются
        using (MemoryStream ms = new MemoryStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = socket.Receive(buffer)) > 0)
            {
                ms.Write(buffer, 0, bytesRead);
                if (bytesRead < 1024) break; // Если получили меньше 1024 байт, выходим из цикла
            }
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
    public static JObject GetMessageJSON(this Socket socket){
        string json = socket.GetMessage();
        try
        {
            return JObject.Parse(json);
        }
        catch (Exception ex)
        {
            ex.ConsoleWriteLine();
            Core.Log(json, ConsoleColor.Red);
        }
        return new JObject();
    }


    public static void SendMessage(this Socket socket, string text)
    {
        socket.Send(Encoding.UTF8.GetBytes(text));
    }
    public static void SendMessage(this Socket socket, JObject jObject) => socket.SendMessage(jObject.ToString());

    public static void ConsoleWriteLine(this Exception exception)
    {
        Core.Log($"Ошибка: {exception.Message}\n{exception.StackTrace}", ConsoleColor.Red);
    }
    public static void Log(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    public static void Log(string text)
    {
        Console.WriteLine(text);
    }




    public static bool IsType(this JObject jObject, string type) => jObject.ContainsKey("type") && 
                                                                    jObject["type"].Type == JTokenType.String &&
                                                                    (string)jObject["type"] == type;
    public static string? GetString(this JObject jObject, string key) => jObject.ContainsKey(key) &&
                                                                         jObject[key].Type == JTokenType.String ? (string)jObject[key] : null;
}
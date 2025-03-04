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
        byte[] sizeBuffer = new byte[4];
        int received = socket.Receive(sizeBuffer);
        if (received != 4)
            throw new Exception("Не удалось получить размер сообщения");

        // Преобразуем размер из Big Endian (Network Byte Order) в UInt32
        uint size = (uint)(sizeBuffer[0] << 24 |
                           sizeBuffer[1] << 16 |
                           sizeBuffer[2] << 8 |
                           sizeBuffer[3]);

        byte[] buffer = new byte[size];
        int totalReceived = 0;

        while (totalReceived < size)
        {
            int bytes = socket.Receive(buffer, totalReceived, (int)size - totalReceived, SocketFlags.None);
            if (bytes == 0) // Если соединение закрылось
                throw new Exception("Соединение было закрыто");

            totalReceived += bytes;
        }

        return Encoding.UTF8.GetString(buffer);
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
        byte[] data = Encoding.UTF8.GetBytes(text);
        byte[] size = BitConverter.GetBytes((uint)data.Length);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(size); // Преобразуем в Big Endian, если используется Little Endian

        //TODO объеденить в 1 сообщение
        socket.Send(size);  // Отправляем размер
        socket.Send(data);  // Отправляем само сообщение
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
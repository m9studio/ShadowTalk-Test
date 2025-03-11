using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;

public static class Core
{
    public const ushort ServerPort = 55555;
    private const ushort clientPort = 55600;
    private static ushort clientCount = 0;
    public static int NewClientPort()
    {
        clientCount++;
        return clientPort + clientCount;
    }


    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
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
}
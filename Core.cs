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
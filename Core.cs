﻿using System.Net.Sockets;
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
    public static void SendMessage(this Socket socket, string text)
    {
        socket.Send(Encoding.UTF8.GetBytes(text));
    }

    public static void ConsoleWriteLine(this Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Ошибка: {exception.Message}\n{exception.StackTrace}");
        Console.ResetColor();
    }
}
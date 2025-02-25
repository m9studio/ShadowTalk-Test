using System.Diagnostics;

class Program
{
    //TODO добавить комментарии, чтобы не запутаться
    //TODO после успешных тестов, разделить на 2 проекта Server и Client
    static void Main(string[] args)
    {
        int i = 0;
        Console.SetWindowSize(32, 24);
        Console.SetBufferSize(32, 24);
        if (args.Length == 0)
        {
            Console.Title = "Server";
            Console.SetWindowPosition(0, 0);
            Server server = new Server(33300);
            server.Open();
            i++;


            while (true)
            {
                string? line = Console.ReadLine();
                if (line != null && line.Length > 1)
                {
                    string clientName = line;
                    //ushort clientPort = (ushort)(33300 + i);



                    // Формируем относительный путь к .exe
                    string relativePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        @"ShadowTalk-Test.exe" // путь относительно этой директории
                    );

                    // Печать для отладки
                    //Console.WriteLine($"Запуск: {relativePath}");

                    // Запускаем процесс
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = relativePath, // передаем относительный путь
                        Arguments = $" {clientName} {i}",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    });
                    i++;
                }
            }

        }
        else if (args.Length == 2)
        {
            Console.Title = args[0];
            try
            {
                Client client = new Client(args[0], (ushort)(33300 + int.Parse(args[1])));
                client.ConnectToServer("127.0.0.1", 33300);
                while (true)
                {
                    string? line = Console.ReadLine();
                    if (line == null) continue;
                    string[] command = line.Split(" ");
                    if (command.Length == 2)
                    {
                        client.SendMessage(command[0], command[1]);
                    }
                    else if (command.Length == 1 && command[0] == "stop")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}

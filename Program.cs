using System.Diagnostics;

class Program
{
    //TODO добавить комментарии, чтобы не запутаться
    //TODO после успешных тестов, разделить на 2 проекта Server и Client
    static/* */void Main(string[] args)
    {
        ushort Port = 55500;
        int i = 0;
        Console.SetWindowSize(36, 30);
        Console.SetBufferSize(36, 10000);
        if (args.Length == 0)
        {
            Console.Title = "Server";
            Console.SetWindowPosition(0, 0);
            Server server = new Server(Port);
            server.Open();
            i++;


            while (true)
            {
                string? line = Console.ReadLine();
                
                if (line != null && line.Length > 1)
                {
                    string[] a = line.Split(' ');
                    if(a.Length > 1)
                    {
                        if (a[0] == "log")
                        {
                            server.Log(a[1]);
                        }
                        continue;
                    }
                    if(line == "log")
                    {
                        continue;
                    }




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
                Client client = new Client(args[0], (ushort)(Port + int.Parse(args[1])));
                client.Start("127.0.0.1", Port);
                while (true)
                {
                    string? line = Console.ReadLine();
                    if (line == null) continue;
                    string[] command = line.Split(" ");
                    if (command.Length > 1)
                    {
                        if (command[0] == "log")
                        {
                            Task.Run(() => client.Log(command[1]));
                            continue;
                        }
                        if (command[0] == "udp")
                        {
                            Task.Run(() => client.UdpOpen(command[1]));
                            continue;
                        }




                        string text = "";
                        for(int j = 1; j < command.Length; j++)
                        {
                            text += command[j];
                            if(j + 1 < command.Length)
                            {
                                text += ' ';
                            }
                        }
                        Task.Run(() =>
                        {
                            if(!client.SendMessage(command[0], text))
                            {
                                Core.Log("Не удалось отправить письмо для " + command[0]);
                            }
                        });
                    }
                    else if (command.Length == 1)
                    {
                        if (command[0] == "stop") break;
                        if (command[0] == "log") client.Log();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConsoleWriteLine();
                Console.ReadLine();
            }
        }
    }
}

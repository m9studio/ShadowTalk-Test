using System.Net.Sockets;
using System.Net;

class Server
{
    private Socket _serverSocket;
    private Dictionary<string, Socket> _clients = new();

    public Server(ushort port)
    {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        _serverSocket.Listen(10);
    }

    public bool Open()
    {
        Task.Run(AcceptClientsAsync);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Сервер запущен...");
        Console.ResetColor();
        return true;
    }

    private async Task AcceptClientsAsync()
    {
        //Console.WriteLine(1);
        while (true)
        {
            Socket clientSocket = await _serverSocket.AcceptAsync();
            //Выводим прослушку сокета в отдельный поток, чтобы не лочить данный цикл
            //TODO может сделать отдельный метод для регистрации и уже от него выводить в HandleClientAsync?
            _ = Task.Run(() => HandleClientAsync(clientSocket));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("new connect");
            Console.ResetColor();
        }
    }

    private async Task HandleClientAsync(Socket clientSocket)
    {
        string name = "none";
        IPEndPoint remoteIpEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
        IPEndPoint localIpEndPoint = clientSocket.LocalEndPoint as IPEndPoint;
        try
        {
            while (clientSocket.Connected)
            {
                string request = clientSocket.GetMessage();
                string[] parts = request.Split(' ');

                //Пользователь хочет себя зарегистрировать
                if (parts[0] == "register") 
                {
                    name = parts[1];
                    _clients[name] = clientSocket;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{name}: подключился [{clientSocket.RemoteEndPoint}]");
                    Console.ResetColor();
                }
                //Пользователь запрашивает подключение
                else if (parts[0] == "connect")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{name}: пытается связаться с [{parts[1]}], его адресс [{remoteIpEndPoint.Address}:{parts[2]}]");
                    Console.ResetColor();
                    if (_clients.ContainsKey(parts[1]))
                    {
                        _clients[parts[1]].SendMessage("connect " + name + " " + remoteIpEndPoint.Address + " " + parts[2]);
                    }
                }
                else
                {
                    Console.WriteLine(name + ": " + request);
                }
            }
        }
        catch (Exception ex)
        {
            ex.ConsoleWriteLine();
        }
        Console.WriteLine($"{name} отключился [{clientSocket.RemoteEndPoint}]");
    }
}

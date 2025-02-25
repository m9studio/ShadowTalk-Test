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
        Console.WriteLine("Сервер запущен...");
        return true;
    }

    private async Task AcceptClientsAsync()
    {
        //Console.WriteLine(1);
        while (true)
        {
            Socket clientSocket = await _serverSocket.AcceptAsync();
            _ = Task.Run(() => HandleClientAsync(clientSocket));
            Console.WriteLine("new connect");
        }
    }

    private async Task HandleClientAsync(Socket clientSocket)
    {
        string name = "none";
        try
        {
            while (clientSocket.Connected)
            {
                string request = clientSocket.GetMessage();
                Console.WriteLine(request);
                string[] parts = request.Split(' ');

                if (parts[0] == "register")
                {
                    name = parts[1];
                    _clients[name] = clientSocket;
                    Console.WriteLine($"{name} подключился [{clientSocket.RemoteEndPoint}]");
                }
                else if (parts[0] == "connect")
                {
                    if (_clients.ContainsKey(parts[1]))
                    {
                        _clients[parts[1]].SendMessage(parts[2]);
                    }
                    // string clientList = string.Join(",", _clients.Keys);
                    //clientSocket.SendMessage($"clients:{clientList}");
                }
            }
            Console.WriteLine($"{name} отключился [{clientSocket.RemoteEndPoint}]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

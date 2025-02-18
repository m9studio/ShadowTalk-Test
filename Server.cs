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
        while (true)
        {
            Socket clientSocket = await _serverSocket.AcceptAsync();
            _ = HandleClientAsync(clientSocket);
        }
    }

    private async Task HandleClientAsync(Socket clientSocket)
    {
        try
        {
            string request = clientSocket.GetMessage();
            string[] parts = request.Split(':');

            if (parts[0] == "register")
            {
                string name = parts[1];
                _clients[name] = clientSocket;
                Console.WriteLine($"Клиент {name} подключился [{clientSocket.RemoteEndPoint}]");
            }
            else if (parts[0] == "get_clients")
            {
                string clientList = string.Join(",", _clients.Keys);
                clientSocket.SendMessage($"clients:{clientList}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

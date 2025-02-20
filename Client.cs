using System.Net;
using System.Net.Sockets;

class Client
{
    private Socket _serverSocket;
    private string _name;
    private Dictionary<string, Socket> _peers = new();
    private ushort _localPort;

    public Client(string name, ushort port)
    {
        _name = name;
        _localPort = port;
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void ConnectToServer(string serverIp, ushort serverPort)
    {
        _serverSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
        _serverSocket.SendMessage($"register {_name} {_localPort}");

        Task.Run(StartListeningAsync); // Клиент начинает слушать входящие соединения
    }

    private async Task StartListeningAsync()
    {
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, _localPort));
        listener.Listen(10);

        while (true)
        {
            Socket incomingClient = await listener.AcceptAsync();
            Task.Run(() => HandleIncomingConnectionAsync(incomingClient));
        }
    }

    private async Task HandleIncomingConnectionAsync(Socket incomingClient)
    {
        string peerName = incomingClient.GetMessage();
        _peers[peerName] = incomingClient;
        Console.WriteLine($"{_name}: Подключился {peerName}");

        Task.Run(() => ListenToPeer(peerName, incomingClient));
    }

    private async Task ListenToPeer(string peerName, Socket peerSocket)
    {
        try
        {
            while (true)
            {
                string message = peerSocket.GetMessage();
                Console.WriteLine($"{_name} <- {peerName}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{_name}: Ошибка при приёме сообщений от {peerName} - {ex.Message}");
            _peers.Remove(peerName);
        }
    }


    public void ConnectToPeer(string peerIp, ushort peerPort, string peerName)
    {
        Socket peerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        peerSocket.Connect(new IPEndPoint(IPAddress.Parse(peerIp), peerPort));
        peerSocket.SendMessage(_name); // Отправляем своё имя, чтобы другой клиент знал, кто подключился
        _peers[peerName] = peerSocket;

        Console.WriteLine($"{_name}: Подключился к {peerName}");
    }


    public void SendMessage(string user, string text)
    {
        if (_peers.ContainsKey(user))
            _peers[user].SendMessage(text);
        else
            _serverSocket.SendMessage($"connect {user} {_localPort}");
    }
}

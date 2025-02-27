using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;

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
        JObject json = new JObject();
        json["name"] = _name;
        json["port"] = _localPort;
        json["type"] = "register";


        _serverSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
        _serverSocket.SendMessage(json.ToString());

        Task.Run(StartListeningAsyncServer); // Клиент начинает слушать входящие сообщения от сервера
        Task.Run(StartListeningAsync); // Клиент начинает слушать входящие соединения от других пользователей
    }
    private async Task StartListeningAsyncServer()
    {
        try
        {
            while (_serverSocket.Connected)
            {
                string request = _serverSocket.GetMessage();
                Core.Log("Server: " + request, ConsoleColor.DarkYellow);

                string[] parts = request.Split(' ');
                if(parts.Length == 4 && parts[0] == "connect")
                {
                    ConnectToPeer(parts[2], ushort.Parse(parts[3]), parts[1]);
                }
            }
        }
        catch (Exception ex)
        {
            ex.ConsoleWriteLine();
        }
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
        Core.Log($"{peerName} подключился", ConsoleColor.Yellow);

        Task.Run(() => ListenToPeer(peerName, incomingClient));
    }

    private async Task ListenToPeer(string peerName, Socket peerSocket)
    {
        try
        {
            while (true)
            {
                string message = peerSocket.GetMessage();
                Core.Log($"{peerName}: {message}", ConsoleColor.Green);
            }
        }
        catch (Exception ex)
        {
            ex.ConsoleWriteLine();
        }
        Core.Log($"{peerName} отключился");
        _peers.Remove(peerName);
    }


    public void ConnectToPeer(string peerIp, ushort peerPort, string peerName)
    {
        Core.Log($"Подключаемся к {peerName}", ConsoleColor.Yellow);
        Socket peerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        peerSocket.Connect(new IPEndPoint(IPAddress.Parse(peerIp), peerPort));
        peerSocket.SendMessage(_name); // Отправляем своё имя, чтобы другой клиент знал, кто подключился
        _peers[peerName] = peerSocket;

        Task.Run(() => ListenToPeer(peerName, peerSocket));

    }

    public bool SendMessage(string user, string text)
    {
        JObject json = new JObject();
        json["text"] = text;


        //Если есть соединение с данным пользователем, то отправляем сообщение
        if (_peers.ContainsKey(user)) 
        {
            _peers[user].SendMessage(json.ToString());
            return true;
        }
        //Иначе запрашиваем сервер, на соединение с пользователем
        else
        {
            //TODO лочить _peers[user] пока не появится соединение
            _serverSocket.SendMessage($"connect {user} {_localPort}");
            //ждем подлючение от user, после чего отправляем сообщение
            for(int i = 0; i < 6000; i++)
            {
                Thread.Sleep(10);//10 * 6 (6000 / 1000) = 60 сек на ожидание подключение
                if(_peers.ContainsKey(user))
                {
                    _peers[user].SendMessage(text);
                    return true;
                }
            }
            return false;
        }
    }
}

using Newtonsoft.Json.Linq;
using Struct;
using System.Net;
using System.Net.Sockets;

class Client
{
    private SocketHandler _serverSocket;
    private string _name;
    private Dictionary<string, SocketHandler> _peers = new();
    private ushort _localPort;

    public Client(string name, ushort port)
    {
        _name = name;
        _localPort = port;
        _serverSocket = new SocketHandler(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Start(string serverIp, ushort serverPort)
    {
        _serverSocket.Connect(serverIp, serverPort);
        _serverSocket.Send(new ClientToServerRegister(_name, _localPort).ToString());

        _ = Task.Run(ListenServer); // Клиент начинает слушать входящие сообщения от сервера
        _ = Task.Run(StartListeningAsync); // Клиент начинает слушать входящие соединения от других пользователей
    }
    /// <summary>
    /// Прослушиваем сервер
    /// </summary>
    /// <returns></returns>
    private void ListenServer()
    {
        try
        {
            while (_serverSocket.Connected)
            {
                JObject request = _serverSocket.GetJObject();
                ServerToClientConnect? connect = ServerToClientConnect.Convert(request);
                if (connect != null)
                {
                    Core.Log($"Server: Запрос на подключение\n{request}", ConsoleColor.DarkYellow);
                    _ = Task.Run(() => ConnectToPeer(connect.IP, connect.Port, connect.Name, connect.Key));
                }
                else
                {
                    Core.Log($"Server: Неизвестный или неправильный запрос\n{request}", ConsoleColor.DarkRed);
                }
            }
        }
        catch (Exception ex)
        {
            ex.ConsoleWriteLine();
        }
    }
    /// <summary>
    /// Подключаемся к другому пользователю
    /// </summary>
    /// <param name="peerIp"></param>
    /// <param name="peerPort"></param>
    /// <param name="peerName"></param>
    /// <param name="peerKey"></param>
    /// <returns></returns>
    public void ConnectToPeer(string peerIp, ushort peerPort, string peerName, string peerKey)
    {
        Core.Log($"Подключаемся к {peerName}", ConsoleColor.Yellow);
        SocketHandler peerSocket = new SocketHandler(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
        peerSocket.Socket.Connect(new IPEndPoint(IPAddress.Parse(peerIp), peerPort));

        peerSocket.Send(new ClientToClientConnect(_name, "").ToString()); // Отправляем своё имя, чтобы другой клиент знал, кто подключился
        _peers[peerName] = peerSocket;

        ListenToPeer(peerName, peerSocket);
    }
    /// <summary>
    /// Ожидаем подключение других пользователей
    /// </summary>
    /// <returns></returns>
    private async Task StartListeningAsync()
    {
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, _localPort));
        listener.Listen(10);

        while (true)
        {
            Socket incomingClient = await listener.AcceptAsync();
            _ = Task.Run(() => HandleIncomingConnection(new SocketHandler(incomingClient)));
        }
    }
    /// <summary>
    /// Верефикация другого пользователя
    /// </summary>
    /// <param name="incomingClient"></param>
    /// <returns></returns>
    private void HandleIncomingConnection(SocketHandler incomingClient)
    {
        JObject request = incomingClient.GetJObject();
        Core.Log($"{request}", ConsoleColor.DarkYellow);
        ClientToClientConnect? connect = ClientToClientConnect.Convert(request);
        if (connect != null)
        {
            _peers[connect.Name] = incomingClient;
            ListenToPeer(connect.Name, incomingClient);
        }
        else
        {
            Core.Log("Неправильный тип запроса", ConsoleColor.DarkRed);
        }
    }
    /// <summary>
    /// Прослушка другого пользователя
    /// </summary>
    /// <param name="peerName"></param>
    /// <param name="peerSocket"></param>
    /// <returns></returns>
    private void ListenToPeer(string peerName, SocketHandler peerSocket)
    {
        try
        {
            while (true)
            {
                JObject request = peerSocket.GetJObject();
                ClientToClientMessage? message = ClientToClientMessage.Convert(request);
                if (message != null)
                {
                    Core.Log($"{peerName}: {message.Text}", ConsoleColor.Green);
                }
                else
                {
                    Core.Log($"{peerName}: Неизвестный запрос:{request}", ConsoleColor.DarkRed);
                }
            }
        }
        catch (Exception ex)
        {
            ex.ConsoleWriteLine();
        }
        Core.Log($"{peerName} отключился");
        _peers.Remove(peerName);
    }





    public bool SendMessage(string user, string text)
    {
        ClientToClientMessage message = new ClientToClientMessage(text);

        //Если есть соединение с данным пользователем, то отправляем сообщение
        if (_peers.ContainsKey(user)) 
        {
            _peers[user].Send(message.ToString());
            return true;
        }
        //Иначе запрашиваем сервер, на соединение с пользователем
        else
        {
            //TODO лочить _peers[user] пока не появится соединение так-же учесть верефикацию через key
            _serverSocket.Send(new ClientToServerConnect(user, "").ToString());
            //ждем подлючение от user, после чего отправляем сообщение
            for (int i = 0; i < 6000; i++)
            {
                Thread.Sleep(10);//10 * 6 (6000 / 1000) = 60 сек на ожидание подключение
                if(_peers.ContainsKey(user))
                {
                    _peers[user].Send(message.ToString());
                    return true;
                }
            }
            return false;
        }
    }
}

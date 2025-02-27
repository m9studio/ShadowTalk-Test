﻿using Newtonsoft.Json.Linq;
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
                JObject request = _serverSocket.GetMessageJSON();
                Core.Log($"Server: {request}", ConsoleColor.DarkYellow);
                if(request.IsType("connect"))
                {
                    string? name = request.GetString("name");
                    string? port = request.GetString("port");
                    string? ip = request.GetString("ip");
                    if (name != null && port != null && ip != null)
                    {
                        ConnectToPeer(ip, ushort.Parse(port), name);
                    }
                    else
                    {
                        Core.Log($"Server: Неправильный запрос", ConsoleColor.DarkRed);
                    }
                }
                else
                {
                    Core.Log($"Server: Неизвестный запрос", ConsoleColor.DarkRed);
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
        JObject request = incomingClient.GetMessageJSON();
        Core.Log($"{request}", ConsoleColor.DarkYellow);
        if (request.IsType("connect"))
        {
            string? name = request.GetString("name");
            if(name != null)
            {
                _peers[name] = incomingClient;
                Task.Run(() => ListenToPeer(name, incomingClient));
            }
            else
            {
                Core.Log("Неправильный запрос", ConsoleColor.DarkRed);
            }
        }
        else
        {
            Core.Log("Неправильный тип запроса", ConsoleColor.DarkRed);
        }

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

        JObject json = new JObject();
        json["name"] = _name;
        json["type"] = "connect";

        peerSocket.SendMessage(json); // Отправляем своё имя, чтобы другой клиент знал, кто подключился
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
            JObject jsonConnect = new JObject();
            jsonConnect["type"] = "connect";
            jsonConnect["user"] = user;
            jsonConnect["port"] = _localPort.ToString();

            //TODO лочить _peers[user] пока не появится соединение
            _serverSocket.SendMessage(jsonConnect);
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

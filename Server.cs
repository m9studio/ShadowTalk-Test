using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json.Linq;
using Struct;

class Server
{
    private class ServerUser
    {
        public string Name;
        public SocketHandler Socket;
        public int Port;
        public Logger Logger;
    }


    private Logger logger;


    private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private Dictionary<string, ServerUser> _clients = new();

    public Server(Logger logger)
    {
        this.logger = logger;
    }
    public Server(Logger logger, ushort port):this(logger)
    {
        Open(port);
    }

    public bool Open(ushort port)
    {
        _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        _serverSocket.Listen(10);
        Task.Run(AcceptClientsAsync);
        logger.Log("Сервер запущен..."/*, ConsoleColor.Green*/);
        return true;
    }

    private async Task AcceptClientsAsync()
    {
        while (true)
        {
            //Ожидаем новое входящее подключение
            Socket clientSocket = await _serverSocket.AcceptAsync();
            //Генерируем UUID для отслеживания логов
            ServerUser user = new ServerUser();
            user.Logger = ((LoggerServer)logger).Clone();
            user.Logger.Log("Новое подключение");

            //Выводим прослушку сокета в отдельный поток, чтобы не лочить данный цикл
            _ = Task.Run(() => HandleClient(new SocketHandler(clientSocket, user.Logger), user));
        }
    }

    private void HandleClient(SocketHandler clientSocket, ServerUser user)
    {
        user.Socket = clientSocket;



        //string? name = null;
        IPEndPoint remoteIpEndPoint = clientSocket.Socket.RemoteEndPoint as IPEndPoint;
        //IPEndPoint localIpEndPoint = clientSocket.LocalEndPoint as IPEndPoint;
        try
        {
            //TODO ожидание минута
            JObject request = clientSocket.GetJObject();

            ClientToServerRegister? register = ClientToServerRegister.Convert(request);
            if (register == null)
            {
                user.Logger.Log("Неправильный запрос");
                return;
            }
            if (_clients.ContainsKey(register.Name))
            {
                user.Logger.Log("Недопустимое имя");
                return;
            }
            user.Port = register.Port;
            user.Name = register.Name;
            _clients[user.Name] = user;
            user.Logger.Log("Прошел регистрацию");


            while (clientSocket.Connected)
            {
                request = clientSocket.GetJObject();
                ClientToServerConnect? connect = ClientToServerConnect.Convert(request);
                if (connect != null)
                {
                    if (_clients.ContainsKey(connect.Name))
                    {
                        user.Logger.Log("Хочет связаться с пользователем");
                        _clients[connect.Name].Socket.Send(new ServerToClientConnect(user.Name, user.Port, remoteIpEndPoint.Address.ToString(), connect.Key));
                    }
                    else user.Logger.Log("Пытается связаться с неизвестным пользователем");
                }
                else user.Logger.Log("Неизвестный или неправильный запрос");
            }
            user.Logger.Log("Отключился");
        }
        catch (Exception ex)
        {
            user.Logger.Log(ex.Message);
        }
    }
}

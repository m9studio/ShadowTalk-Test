using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json.Linq;

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
        Core.Log("Сервер запущен...", ConsoleColor.Green);
        return true;
    }

    private async Task AcceptClientsAsync()
    {
        while (true)
        {
            //Ожидаем новое входящее подключение
            Socket clientSocket = await _serverSocket.AcceptAsync();
            //Генерируем UUID для отслеживания логов
            string UUID = Guid.NewGuid().ToString();

            Core.Log($"{UUID}: New connect [{clientSocket.RemoteEndPoint}]");
            //Выводим прослушку сокета в отдельный поток, чтобы не лочить данный цикл
            _ = Task.Run(() => HandleClientAsync(clientSocket, UUID));
        }
    }

    private async Task HandleClientAsync(Socket clientSocket, string UUID)
    {
        string? name = null;
        IPEndPoint remoteIpEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
        IPEndPoint localIpEndPoint = clientSocket.LocalEndPoint as IPEndPoint;
        try
        {
            JObject request = clientSocket.GetMessageJSON();
            Core.Log($"{UUID}: {request}");
            name = request.GetString("name");
            if (request.IsType("register") && name != null)
            {
                name = name.Trim();
                if (_clients.ContainsKey(name) || string.IsNullOrEmpty(name))
                {
                    Core.Log($"{UUID}: Недопустимое имя", ConsoleColor.DarkRed);
                    return;
                }
            }
            else
            {
                Core.Log($"{UUID}: Неправильный запрос", ConsoleColor.DarkRed);
                return;
            }
            _clients[name] = clientSocket;
            Core.Log($"{UUID} [{name}]: Подключился", ConsoleColor.Green);


            while (clientSocket.Connected)
            {
                request = clientSocket.GetMessageJSON();
                Core.Log($"{UUID} [{name}]: {request}");
                if (request.IsType("connect"))
                {
                    string? userName = request.GetString("user");
                    string? port = request.GetString("port");
                    if (userName != null && port != null)
                    {
                        Core.Log($"{UUID} [{name}]: пытается связаться с [{userName}], его адресс [{remoteIpEndPoint.Address}:{port}]", ConsoleColor.Yellow);
                        if (_clients.ContainsKey(userName))
                        {
                            JObject json = new JObject();
                            json["type"] = "connect";
                            json["name"] = userName;
                            json["ip"] = remoteIpEndPoint.Address.ToString();
                            json["port"] = port;

                            _clients[userName].SendMessage(json);
                        }
                    }
                    else
                    {
                        Core.Log($"{UUID} [{name}]: Неправильный запрос", ConsoleColor.DarkRed);
                    }
                }
                else
                {
                    Core.Log($"{UUID} [{name}]: Не известный запрос", ConsoleColor.DarkRed);
                }
            }
        }
        catch (Exception ex)
        {
            if(name != null)
            {
                Core.Log($"{UUID} [{name}]:", ConsoleColor.Red);
            }
            else
            {
                Core.Log($"{UUID}:", ConsoleColor.Red);
            }
            ex.ConsoleWriteLine();
        }
        Core.Log($"{UUID} [{name}]: Отключился");
    }
}

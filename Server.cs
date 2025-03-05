using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json.Linq;
using Struct;

class Server
{
    private class ServerUser
    {
        public string Name;
        public string UUID;
        public SocketHandler Socket;
        public ushort Port;

        public void LogError(string text) => Core.Log($"{UUID} [{Name}]: {text}", ConsoleColor.Red);
        public void LogError(string text, JObject jObject) => LogError($"{text}\n{jObject}");
        public void LogError(Exception exception) => LogError($"Ошибка: {exception.Message}\n{exception.StackTrace}");

        public void LogSuccess(string text) => Core.Log($"{UUID} [{Name}]: {text}", ConsoleColor.Green);
        public void LogSuccess(string text, JObject jObject) => LogSuccess($"{text}\n{jObject}");

        public void LogWarn(string text) => Core.Log($"{UUID} [{Name}]: {text}", ConsoleColor.Yellow);
        public void LogWarn(string text, JObject jObject) => LogWarn($"{text}\n{jObject}");



        public void LogErrorA(string text) => Core.Log($"{UUID} [{Name}]: {text}", ConsoleColor.DarkRed);
        public void LogErrorA(string text, JObject jObject) => LogErrorA($"{text}\n{jObject}");
        public void LogErrorA(Exception exception) => LogErrorA($"Ошибка: {exception.Message}\n{exception.StackTrace}");

        public void LogSuccessA(string text) => Core.Log($"{UUID} [{Name}]: {text}", ConsoleColor.DarkGreen);
        public void LogSuccessA(string text, JObject jObject) => LogSuccessA($"{text}\n{jObject}");

        public void LogWarnA(string text) => Core.Log($"{UUID} [{Name}]: {text}", ConsoleColor.DarkYellow);
        public void LogWarnA(string text, JObject jObject) => LogWarnA($"{text}\n{jObject}");
    }
    




    private Socket _serverSocket;
    private Dictionary<string, ServerUser> _clients = new();

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
            ServerUser user = new ServerUser();
            user.UUID = Guid.NewGuid().ToString();

            user.LogSuccessA("Новое подключение");

            //Выводим прослушку сокета в отдельный поток, чтобы не лочить данный цикл
            _ = Task.Run(() => HandleClient(new SocketHandler(clientSocket), user));
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
                user.LogErrorA("Неправильный запрос", request);
                return;
            }
            if (_clients.ContainsKey(register.Name))
            {
                user.LogErrorA("Недопустимое имя", request);
                return;
            }
            user.Port = register.Port;
            user.Name = register.Name;
            _clients[user.Name] = user;
            user.LogSuccessA("Прошел регистрацию", request);


            while (clientSocket.Connected)
            {
                request = clientSocket.GetJObject();
                ClientToServerConnect? connect = ClientToServerConnect.Convert(request);
                if (connect != null)
                {
                    if (_clients.ContainsKey(connect.Name))
                    {
                        user.LogSuccess("Хочет связаться с пользователем", request);
                        _clients[connect.Name].Socket.Send(new ServerToClientConnect(user.Name, user.Port, remoteIpEndPoint.Address.ToString(), connect.Key).ToString());
                    }
                    else user.LogWarn("Пытается связаться с неизвестным пользователем", request);
                }
                else user.LogWarn("Неизвестный или неправильный запрос", request);
            }
            user.LogError("Отключился");
        }
        catch (Exception ex)
        {
            user.LogError(ex);
        }
    }
}

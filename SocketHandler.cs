using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Struct;

public class SocketHandler
{
    private List<string> trace = new List<string>();
    public void Log()
    {
        foreach(string item in trace)
        {
            Core.Log(item, ConsoleColor.Blue);
        }
    }


    private Socket socket;
    public Socket Socket => socket;
    public bool Connected => socket.Connected;
    public SocketHandler(Socket socket)
    {
        this.socket = socket;
    }
    public SocketHandler(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : this(new Socket(addressFamily, socketType, protocolType)) { }
    public void Connect(EndPoint remoteEP) => socket.Connect(remoteEP);
    public void Connect(IPAddress address, int port) => socket.Connect(new IPEndPoint(address, port));
    public void Connect(string address, int port) => socket.Connect(IPAddress.Parse(address), port);

    public void Bind(EndPoint localEP) => socket.Bind(localEP);


    public void Send(byte[] bytes)
    {
        byte[] size = BitConverter.GetBytes((uint)bytes.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(size); // Преобразуем в Big Endian, если используется Little Endian

        //socket.Send(size);  // Отправляем размер
        //socket.Send(bytes);  // Отправляем само сообщение

        byte[] message = new byte[size.Length + bytes.Length];
        Buffer.BlockCopy(size, 0, message, 0, size.Length);
        Buffer.BlockCopy(bytes, 0, message, size.Length, bytes.Length);

        socket.Send(message);

        trace.Add("Send: " + hex(message));
    }
    public void Send(string text)
    {
        Send(Encoding.UTF8.GetBytes(text));
        trace.Add("Send: " + text);
    }
    public void Send(JObject data) => Send(data.ToString());
    public void Send(StructData data) => Send(data.ToString());


    public byte[] GetBytes()
    {
        byte[] sizeBuffer = new byte[4];
        int received = socket.Receive(sizeBuffer);
        if (received != 4)
            throw new Exception("Не удалось получить размер сообщения");
        // Преобразуем размер из Big Endian (Network Byte Order) в UInt32
        uint size = (uint)(sizeBuffer[0] << 24 |
                           sizeBuffer[1] << 16 |
                           sizeBuffer[2] << 8 |
                           sizeBuffer[3]);
        byte[] buffer = new byte[size];
        int totalReceived = 0;

        while (totalReceived < size)
        {
            int bytes = socket.Receive(buffer, totalReceived, (int)size - totalReceived, SocketFlags.None);
            if (bytes == 0) // Если соединение закрылось
                throw new Exception("Соединение было закрыто");
            totalReceived += bytes;
        }
        trace.Add("Get: " + hex(sizeBuffer) + hex(buffer));
        return buffer;
    }
    public string GetString()
    {
        string data = Encoding.UTF8.GetString(GetBytes());
        trace.Add("Get: " + data);
        return data;
    }
    public JObject GetJObject() => JObject.Parse(GetString());


    private string hex(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
    }
}
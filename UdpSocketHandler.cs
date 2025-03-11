using System.Net;
using System.Net.Sockets;

public class UdpSocketHandler
{
    private Socket socket;
    public int Port => ((IPEndPoint)socket.LocalEndPoint).Port;
    public bool Connected => socket.Connected;

    public UdpSocketHandler()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 0));
    }
    public void Send(byte[] bytes) => socket.Send(bytes);
    public byte[] Get()
    {
        byte[] buffer = new byte[1024];
        int bytesReceived = socket.Receive(buffer);

        byte[] receivedData = new byte[bytesReceived];
        Array.Copy(buffer, receivedData, bytesReceived);
        return receivedData;
    }
    public void Connect(IPEndPoint remoteEP) => socket.Connect(remoteEP);
    public void Close() => socket.Close();
}

internal class LoggerClient : Logger
{
    public LoggerClient(ListBox socket, ListBox message)
    {
        this.socket = socket;
        this.message = message;
    }
    List<string> _socket = new List<string>();
    ListBox socket;
    List<string> _message = new List<string>();
    ListBox message;
    private bool action = false;
    public bool Action
    {
        get => action;
        set
        {
            if (action != value)
            {
                if (value)
                {
                    socket.Invoke(new Action(() =>
                    {
                        foreach (string text in _socket)
                        {
                            foreach (string line in text.Split('\n'))
                            {
                                socket.Items.Add(line);
                            }
                        }
                    }));
                    message.Invoke(new Action(() =>
                    {
                        foreach (string text in _message)
                        {
                            foreach (string line in text.Split('\n'))
                            {
                                message.Items.Add(line);
                            }
                        }
                    }));
                }
                else
                {
                    socket.Invoke(new Action(() => {
                        socket.Items.Clear();
                    }));
                    message.Invoke(new Action(() => {
                        message.Items.Clear();
                    }));
                }
            }
            action = value;
        }
    }
    public override void Log(string text)
    {
        text = text.Replace("\n", "\n\t");
        _socket.Add(text);
        if (action)
        {
            socket.Invoke(new Action(() =>
            {
                foreach (string line in text.Split('\n'))
                {
                    socket.Items.Add(line);
                }
            }));
        }
    }
    public void Message(string user, string text)
    {

        text = user + ":\n" + text.Replace("\n", "\n\t");
        _message.Add(text);
        message.Invoke(new Action(() =>
        {
            foreach (string line in text.Split('\n'))
            {
                message.Items.Add(line);
            }
        }));
    }
    public LoggerClient Clone()
    {
        return new LoggerClient(socket, message);
    }
}

public partial class FormServer : Form
{
    Server server;
    public FormServer()
    {
        InitializeComponent();
        MinimumSize = new Size(500, 300);
        ClientSize = new Size(500, 300);
        listBoxLog.Size = new Size(480, 250);

        server = new Server(new LoggerListBox(listBoxLog));
        Shown += OpenServer;//TODO правильно ли?
    }

    private void OpenServer(object? sender, EventArgs? e)
    {
        server.Open(Core.ServerPort);
    }

    private void btnOpenClient_Click(object sender, EventArgs e)
    {
        new FormClient();
    }
}

public partial class FormServer : Form
{
    private class FormServerLogger : Logger
    {
        private ListBox listBox;
        public FormServerLogger(ListBox listBox)
        {
            this.listBox = listBox;
        }
        public override void Log(string text)
        {
            listBox.Invoke(new Action(() =>
            {
                listBox.Items.Add(text);
            }));
        }
    }
    Server server;
    public FormServer()
    {
        InitializeComponent();
        server = new Server(new FormServerLogger(listBoxLog));
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

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
                foreach(string line in text.Split('\n'))
                {
                    listBox.Items.Add(line);
                }
            }));
        }
    }
    Server server;
    public FormServer()
    {
        InitializeComponent();
        MinimumSize = new Size(500, 300);
        ClientSize = new Size(500, 300);
        listBoxLog.Size = new Size(480, 250);

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

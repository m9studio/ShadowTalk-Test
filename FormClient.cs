public partial class FormClient : Form
{
    Client client = null;
    public FormClient()
    {
        InitializeComponent();
        string? name = ShowDialog("Введите имя");
        if(name == null)
        {
            Close();
            return;
        }
        client = new Client(name, Core.NewClientPort());
        Shown += Connect;//TODO правильно ли?
        Text = name;
        Show();
    }

    void Connect(object? sender, EventArgs? e)
    {
        client.Start("127.0.0.1", Core.ServerPort);
    }

    public string? ShowDialog(string text)
    {
        Form prompt = new Form()
        {
            Width = 250,
            Height = 95,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = text,
            StartPosition = FormStartPosition.CenterScreen
        };
        TextBox textBox = new TextBox() { Left = 5, Top = 5, Width = 225 };
        Button confirmation = new Button() { Text = "Ok", Left = 180, Width = 50, Top = 30, DialogResult = DialogResult.OK };
        confirmation.Click += (sender, e) => { prompt.Close(); };
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);
        prompt.AcceptButton = confirmation;
        return prompt.ShowDialog(this) == DialogResult.OK ? textBox.Text : null;
    }
}

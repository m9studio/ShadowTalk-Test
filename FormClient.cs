using System.Xml.Linq;

public partial class FormClient : Form
{
    Client client = null;
    public FormClient()
    {
        InitializeComponent();
        string? name = ShowDialog("Введите имя");
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
        {
            Close();
            return;
        }
        name = name.Trim();


        client = new Client(name, Core.NewClientPort());
        Shown += Connect;//TODO правильно ли?
        Text = name;
        AddLeftBorder(LogPanel);
        AddBottomBorder(UsersTextBox);

        AddBottomBorder(ChatLabel);
        AddBottomBorder(ChatListBox);

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
    public void AddBottomBorder(Control control, Color color = default, int thickness = 1)
    {
        if (color == default) color = Color.Black;
        Panel border = new Panel
        {
            Height = thickness,
            Dock = DockStyle.Bottom,
            BackColor = color
        };
        control.Controls.Add(border);
        border.BringToFront();
    }
    public void AddTopBorder(Control control, Color color = default, int thickness = 1)
    {
        if (color == default) color = Color.Black;
        Panel border = new Panel
        {
            Height = thickness,
            Dock = DockStyle.Top,
            BackColor = color
        };
        control.Controls.Add(border);
        border.BringToFront();
    }
    public void AddLeftBorder(Control control, Color color = default, int thickness = 1)
    {
        if (color == default) color = Color.Black;
        Panel border = new Panel
        {
            Width = thickness,
            Dock = DockStyle.Left,
            BackColor = color
        };
        control.Controls.Add(border);
        border.BringToFront();
    }
    public void AddRightBorder(Control control, Color color = default, int thickness = 1)
    {
        if (color == default) color = Color.Black;
        Panel border = new Panel
        {
            Width = thickness,
            Dock = DockStyle.Right,
            BackColor = color
        };
        control.Controls.Add(border);
        border.BringToFront();
    }




    private void ChatTextBox_TextChanged(object sender, EventArgs e)
    {
        ChatButton.Enabled = !(string.IsNullOrEmpty(ChatTextBox.Text) || 
                               string.IsNullOrEmpty(ChatTextBox.Text.Trim()));
    }
    private void ChatButton_Click(object sender, EventArgs e)
    {
        //TODO отправка сообщения
    }

    private void UsersTextBox_TextChanged(object sender, EventArgs e)
    {
        //TODO изменям UsersListBox.Items по client и UsersTextBox.Text
    }
}

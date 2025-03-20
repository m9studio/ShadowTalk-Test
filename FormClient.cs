using System.Xml.Linq;

public partial class FormClient : Form
{
    Client client = null;
    LoggerClient logger;
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

        logger = new LoggerClient(LogListBox, ChatListBox);
        client = new Client(name, Core.NewClientPort(), logger);

        Shown += Connect;//TODO правильно ли?
        Text = name;
        AddLeftBorder(LogPanel);
        AddBottomBorder(UsersTextBox);

        AddBottomBorder(ChatLabel);
        AddBottomBorder(ChatListBox);

        UsersTextBox_TextChanged(null, null);

        ChatHide();
        KeyPreview = true;
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
        string text = ChatTextBox.Text;
        ChatTextBox.Text = "";
        Task.Run(() => client.SendMessage(ChatLabel.Text, text));
    }

    private void UsersTextBox_TextChanged(object? sender, EventArgs? e)
    {
        //TODO может переделать  UsersListBox.Items.Add  под свой объект?
        UsersListBox.Items.Clear();
        string startName = UsersTextBox.Text;
        if (string.IsNullOrEmpty(startName) || string.IsNullOrEmpty(startName.Trim()))
        {
            startName = "";
        }
        startName = startName.Trim().ToLower();

        foreach (string user in client._peers.Keys)
        {
            if (user.StartsWith(startName))
            {
                UsersListBox.Items.Add(user);
            }
        }

        if (startName != "" && !client._peers.Keys.Contains(startName))
        {
            UsersListBox.Items.Add(startName + " (new chat)");
        }
    }

    private void ChatHide()
    {
        try
        {
            ((LoggerClient)client._peers[ChatLabel.Text].Logger).Action = false;
        }
        catch (Exception) { }
        ChatTextBox.Text = "";
        ChatTextBox.Enabled = false;
        ChatLabel.Text = "";
        logger.Action = true;
    }
    private void ChatShow(string user)
    {
        try
        {
            ((LoggerClient)client._peers[ChatLabel.Text].Logger).Action = false;
        }
        catch (Exception) { }
        logger.Action = false;


        ChatTextBox.Text = "";
        ChatTextBox.Enabled = true;
        ChatLabel.Text = user;
        try
        {
            ((LoggerClient)client._peers[ChatLabel.Text].Logger).Action = true;
        }
        catch (Exception) { }
    }

    private void UsersListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (UsersListBox.SelectedItem != null)
        {
            string item = (string)UsersListBox.SelectedItem;
            if (client._peers.Keys.Contains(item)){
                ChatShow(item);
            }
            else
            {
                ChatShow(UsersTextBox.Text);
            }
        }
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            ChatHide();
        }
    }
}

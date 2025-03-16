partial class FormClient
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        LogPanel = new Panel();
        LogListBox = new ListBox();
        splitContainer = new SplitContainer();
        UsersListBox = new ListBox();
        UsersTextBox = new TextBox();
        ChatListBox = new ListBox();
        ChatLabel = new Label();
        ChatPanel = new Panel();
        ChatPanelRichTextBox = new Panel();
        ChatTextBox = new TextBox();
        ChatPanelButton = new Panel();
        ChatButton = new Button();
        LogPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        ChatPanel.SuspendLayout();
        ChatPanelRichTextBox.SuspendLayout();
        ChatPanelButton.SuspendLayout();
        SuspendLayout();
        // 
        // LogPanel
        // 
        LogPanel.Controls.Add(LogListBox);
        LogPanel.Dock = DockStyle.Right;
        LogPanel.Location = new Point(534, 0);
        LogPanel.Name = "LogPanel";
        LogPanel.Size = new Size(200, 461);
        LogPanel.TabIndex = 1;
        // 
        // LogListBox
        // 
        LogListBox.BorderStyle = BorderStyle.None;
        LogListBox.Dock = DockStyle.Fill;
        LogListBox.FormattingEnabled = true;
        LogListBox.IntegralHeight = false;
        LogListBox.ItemHeight = 15;
        LogListBox.Location = new Point(0, 0);
        LogListBox.Name = "LogListBox";
        LogListBox.Size = new Size(200, 461);
        LogListBox.TabIndex = 0;
        // 
        // splitContainer
        // 
        splitContainer.BackColor = Color.Black;
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 0);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.AutoScroll = true;
        splitContainer.Panel1.Controls.Add(UsersListBox);
        splitContainer.Panel1.Controls.Add(UsersTextBox);
        splitContainer.Panel1MinSize = 100;
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.AutoScroll = true;
        splitContainer.Panel2.Controls.Add(ChatListBox);
        splitContainer.Panel2.Controls.Add(ChatLabel);
        splitContainer.Panel2.Controls.Add(ChatPanel);
        splitContainer.Panel2MinSize = 250;
        splitContainer.Size = new Size(534, 461);
        splitContainer.SplitterDistance = 150;
        splitContainer.SplitterWidth = 1;
        splitContainer.TabIndex = 2;
        // 
        // UsersListBox
        // 
        UsersListBox.BorderStyle = BorderStyle.None;
        UsersListBox.Dock = DockStyle.Fill;
        UsersListBox.FormattingEnabled = true;
        UsersListBox.IntegralHeight = false;
        UsersListBox.ItemHeight = 15;
        UsersListBox.Location = new Point(0, 16);
        UsersListBox.Name = "UsersListBox";
        UsersListBox.Size = new Size(150, 445);
        UsersListBox.TabIndex = 1;
        // 
        // UsersTextBox
        // 
        UsersTextBox.BorderStyle = BorderStyle.None;
        UsersTextBox.Dock = DockStyle.Top;
        UsersTextBox.Location = new Point(0, 0);
        UsersTextBox.MaxLength = 32;
        UsersTextBox.Name = "UsersTextBox";
        UsersTextBox.PlaceholderText = "User Name";
        UsersTextBox.Size = new Size(150, 16);
        UsersTextBox.TabIndex = 0;
        // 
        // ChatListBox
        // 
        ChatListBox.BorderStyle = BorderStyle.None;
        ChatListBox.Dock = DockStyle.Fill;
        ChatListBox.FormattingEnabled = true;
        ChatListBox.IntegralHeight = false;
        ChatListBox.ItemHeight = 15;
        ChatListBox.Location = new Point(0, 16);
        ChatListBox.Name = "ChatListBox";
        ChatListBox.Size = new Size(383, 370);
        ChatListBox.TabIndex = 2;
        // 
        // ChatLabel
        // 
        ChatLabel.BackColor = SystemColors.Window;
        ChatLabel.Dock = DockStyle.Top;
        ChatLabel.Location = new Point(0, 0);
        ChatLabel.Name = "ChatLabel";
        ChatLabel.Size = new Size(383, 16);
        ChatLabel.TabIndex = 1;
        ChatLabel.Text = "User Name";
        ChatLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // ChatPanel
        // 
        ChatPanel.Controls.Add(ChatPanelRichTextBox);
        ChatPanel.Controls.Add(ChatPanelButton);
        ChatPanel.Dock = DockStyle.Bottom;
        ChatPanel.Location = new Point(0, 386);
        ChatPanel.Name = "ChatPanel";
        ChatPanel.Size = new Size(383, 75);
        ChatPanel.TabIndex = 0;
        // 
        // ChatPanelRichTextBox
        // 
        ChatPanelRichTextBox.BackColor = SystemColors.Control;
        ChatPanelRichTextBox.Controls.Add(ChatTextBox);
        ChatPanelRichTextBox.Dock = DockStyle.Fill;
        ChatPanelRichTextBox.Location = new Point(0, 0);
        ChatPanelRichTextBox.Name = "ChatPanelRichTextBox";
        ChatPanelRichTextBox.Size = new Size(308, 75);
        ChatPanelRichTextBox.TabIndex = 0;
        // 
        // ChatTextBox
        // 
        ChatTextBox.BorderStyle = BorderStyle.None;
        ChatTextBox.Dock = DockStyle.Fill;
        ChatTextBox.Location = new Point(0, 0);
        ChatTextBox.Multiline = true;
        ChatTextBox.Name = "ChatTextBox";
        ChatTextBox.PlaceholderText = "Enter new message";
        ChatTextBox.ScrollBars = ScrollBars.Vertical;
        ChatTextBox.Size = new Size(308, 75);
        ChatTextBox.TabIndex = 0;
        // 
        // ChatPanelButton
        // 
        ChatPanelButton.BackColor = SystemColors.Window;
        ChatPanelButton.Controls.Add(ChatButton);
        ChatPanelButton.Dock = DockStyle.Right;
        ChatPanelButton.Location = new Point(308, 0);
        ChatPanelButton.Name = "ChatPanelButton";
        ChatPanelButton.Size = new Size(75, 75);
        ChatPanelButton.TabIndex = 2;
        // 
        // ChatButton
        // 
        ChatButton.Dock = DockStyle.Top;
        ChatButton.Location = new Point(0, 0);
        ChatButton.Name = "ChatButton";
        ChatButton.Size = new Size(75, 23);
        ChatButton.TabIndex = 0;
        ChatButton.Text = "Отправить";
        ChatButton.UseVisualStyleBackColor = true;
        // 
        // FormClient
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(734, 461);
        Controls.Add(splitContainer);
        Controls.Add(LogPanel);
        MinimumSize = new Size(600, 250);
        Name = "FormClient";
        Text = "Client";
        LogPanel.ResumeLayout(false);
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel1.PerformLayout();
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ChatPanel.ResumeLayout(false);
        ChatPanelRichTextBox.ResumeLayout(false);
        ChatPanelRichTextBox.PerformLayout();
        ChatPanelButton.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
    private Panel LogPanel;
    private SplitContainer splitContainer;
    private TextBox UsersTextBox;
    private ListBox UsersListBox;
    private Panel ChatPanel;
    private Panel ChatPanelButton;
    private Panel ChatPanelRichTextBox;
    private Button ChatButton;
    private Label ChatLabel;
    private ListBox ChatListBox;
    private ListBox LogListBox;
    private TextBox ChatTextBox;
}
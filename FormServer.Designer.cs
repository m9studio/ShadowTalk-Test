using System.Windows.Forms;

partial class FormServer
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
        listBoxLog = new ListBox();
        btnOpenClient = new Button();
        SuspendLayout();
        // 
        // listBoxLog
        // 
        listBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        listBoxLog.FormattingEnabled = true;
        listBoxLog.HorizontalScrollbar = true;
        listBoxLog.ItemHeight = 15;
        listBoxLog.Location = new Point(10, 40);
        listBoxLog.Margin = new Padding(0);
        listBoxLog.Name = "listBoxLog";
        listBoxLog.SelectionMode = SelectionMode.None;
        listBoxLog.Size = new Size(223, 94);
        listBoxLog.TabIndex = 3;
        // 
        // btnOpenClient
        // 
        btnOpenClient.Location = new Point(10, 10);
        btnOpenClient.Margin = new Padding(0);
        btnOpenClient.Name = "btnOpenClient";
        btnOpenClient.Size = new Size(180, 25);
        btnOpenClient.TabIndex = 4;
        btnOpenClient.Text = "Запустить нового клиента";
        btnOpenClient.Click += btnOpenClient_Click;
        // 
        // FormServer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(284, 261);
        Controls.Add(listBoxLog);
        Controls.Add(btnOpenClient);
        Name = "FormServer";
        Text = "Server";
        ResumeLayout(false);
    }

    #endregion
    private ListBox listBoxLog;
    private Button btnOpenClient;
}
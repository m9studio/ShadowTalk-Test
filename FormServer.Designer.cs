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
        SuspendLayout();
        // 
        // listBoxLog
        // 
        listBoxLog.FormattingEnabled = true;
        listBoxLog.ItemHeight = 15;
        listBoxLog.Location = new Point(12, 41);
        listBoxLog.Name = "listBoxLog";
        listBoxLog.Size = new Size(837, 394);
        listBoxLog.TabIndex = 3;
        // 
        // FormServer
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(861, 450);
        Controls.Add(listBoxLog);
        Name = "FormServer";
        Text = "Server";
        ResumeLayout(false);
    }

    #endregion
    private ListBox listBoxLog;
}
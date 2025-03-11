using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class FormServer : Form
{
    Server server;
    public FormServer()
    {
        InitializeComponent();
        server = new Server(Core.ServerPort);
        server.Logger = new FormServerLogger(listBoxLog);
        server.Open();
    }
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
}

internal class LoggerListBox : Logger
{
    private ListBox listBox;
    private string uuid;
    public LoggerListBox(ListBox listBox, string uuid)
    {
        this.listBox = listBox;
        this.uuid = uuid;
    }
    public override void Log(string text)
    {
        text = uuid + ": " + text.Replace("\n", "\n\t");
        listBox.Invoke(new Action(() =>
        {
            foreach (string line in text.Split('\n'))
            {
                listBox.Items.Add(line);
            }
        }));
    }
    public void Reset()
    {
        listBox.Invoke(new Action(listBox.Items.Clear));
    }
    public LoggerListBox Clone()
    {
        return new LoggerListBox(listBox, Guid.NewGuid().ToString());
    }
}

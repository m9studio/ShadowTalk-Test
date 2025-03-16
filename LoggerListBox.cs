internal class LoggerListBox : Logger
{
    private ListBox listBox;
    public LoggerListBox(ListBox listBox)
    {
        this.listBox = listBox;
    }
    public override void Log(string text)
    {
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
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steam_Install_Checker
{
    public static class ListBoxPrompt
    {
        public static string ShowDialog(string text, string caption, List<string> list)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 550,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 1000 };
            ListBox listBox = new ListBox() { Left = 50, Top = 50, Width = 400 };
            list.Insert(0, "none");
            listBox.DataSource = list;
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 400, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(listBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? listBox.Text : "none";
        }
    }
}

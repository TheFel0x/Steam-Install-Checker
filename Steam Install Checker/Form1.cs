using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steam_Install_Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_analyze_Click(object sender, EventArgs e)
        {
            tb_output.Text = "O - Analyzing...\r\n\r\n";
            string[] drives = allDrives().Reverse().ToArray<string>();
            tb_output.Text += "\r\nO - Found drives.\r\n\r\n";
            List<string> locs = locations(drives).Reverse().ToList();
            tb_output.Text += "\r\nO - Found locations.\r\n\r\n";
            //make Dictionary
            Dictionary<string/*drives*/, List<string/*games*/>> gameDictionary = new Dictionary<string, List<string>>();
            foreach (string loc in locs)
            {
                List<string> localGames = new List<string>();
                foreach (string game in Directory.GetDirectories(loc))
                {
                    localGames.Add(game.Substring(loc.Length + 1));
                }
                gameDictionary.Add(loc, localGames);
            }
            //dictionary made
            tb_output.Text += "O - Game dictionary created.\r\n";
            //list games
            foreach (string loc in locs)
            {
                tb_output.Text += "\r\nO - Listing games in " + loc + ".\r\n\r\n";
                foreach (string game in gameDictionary[loc])
                {
                    tb_output.Text += game + "\r\n";
                }
            }
            //find duplicates
            tb_output.Text += "\r\n";
            List<string> allgames = new List<string>();
            List<string> dups = new List<string>();
            foreach (string loc in locs)
            {
                foreach (string game in gameDictionary[loc])
                {
                    if ((allgames.Contains(game)) && !dups.Contains(game)) dups.Add(game);
                    allgames.Add(game);
                }
            }
            foreach (string dup in dups)
            {
                tb_output.Text += "dup:" + dup + "\r\n";
            }
            Dictionary<string/*dup game*/, List<string/*drives*/>> dupDictionary = new Dictionary<string, List<string>>();
            foreach (string dup in dups)
            {
                List<string> driveList = new List<string>();
                foreach (string loc in locs)
                {
                    if (gameDictionary[loc].Contains(dup)) driveList.Add(loc);
                }
                try { dupDictionary.Add(dup, driveList); }
                catch { }
            }
            //dup dictionary finished
            tb_output.Text += "\r\nO - DUP Dictionary created.\r\n";
            foreach (string dup in dups)
            {
                tb_output.Text += "\r\nLocations of " + dup + ":\r\n";
                foreach (string drive in dupDictionary[dup])
                {
                    tb_output.Text += drive + "\r\n";
                }
            }
            if (dups.Count > 0) 
            {
                //yesno: deletion assistant?
                DialogResult dialogResult = MessageBox.Show("Done. All dups found.\nStart duplicate-deletion-Assistant?", "Done.", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (string dup in dups)
                    {
                        string dupLocs = "";
                        foreach (string drive in dupDictionary[dup])
                        {
                            dupLocs += "\r\n" + drive + "\r\n";
                        }
                        string delSelect = ListBoxPrompt.ShowDialog(dup, "Which one do you want to delete? -- " + dup, dupDictionary[dup]);
                        if (delSelect != "none")
                        {
                            try
                            {
                                DialogResult diRes = MessageBox.Show("Are you certain that you want to delete " + delSelect + "\\" + dup + "?", "Delete?", MessageBoxButtons.YesNo);
                                if (diRes == DialogResult.Yes)
                                {
                                    string delPath = delSelect + "\\" + dup;
                                    Directory.Delete(delPath, true);
                                    MessageBox.Show("Done.");
                                }
                                else
                                {
                                    MessageBox.Show("Canceled.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Something Went wrong. That something being:\n" + ex);
                            }
                        }
                    }
                    MessageBox.Show("Everything done.");
                }
            }            
            else
            {
                MessageBox.Show("Done. Congrats! No duplicates found!");
                tb_output.Text += "\r\n\r\nDONE. NO DUPLICATES FOUND.";
            }
        }
        Stack<string> allDrives() 
        {
            char[] alphabet = { 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            Stack<string> output = new Stack<string>();
            foreach(char c in alphabet)
            {
                if(Directory.Exists(c + ":\\"))
                {
                    output.Push(c + ":\\");
                    tb_output.Text += ("O - "+ c + ":\\ found.\r\n");
                }
            }
            return output;
        }
        Stack<string> locations(string[] drives) 
        {
            Stack<string> locs = new Stack<string>();
            //assume and test if there is one on C on the standard directory
            if (Directory.Exists(@"C:\Program Files (x86)\Steam\steamapps\common"))
            {
                locs.Push(@"C:\Program Files (x86)\Steam\steamapps\common");
                tb_output.Text += (@"O - C:\Program Files (x86)\Steam\steamapps\common found." +"\r\n");
            }
            else
            {
                tb_output.Text += (@"X - C:\Program Files (x86)\Steam\steamapps\common NOT FOUND." + "\r\n");
                if (Directory.Exists(@"C:\Program Files\Steam\steamapps\common"))
                {
                    locs.Push(@"C:\Program Files\Steam\steamapps\common");
                    tb_output.Text += (@"O - C:\Program Files\Steam\steamapps\common found." + "\r\n");
                }
                else
                {
                    tb_output.Text += (@"X - C:\Program Files\Steam\steamapps\common NOT FOUND." + "\r\n");
                    //yesno: does the dir have a steam library?
                    DialogResult dialogResult = MessageBox.Show("There was no Steam Library found on the C:\\ drive. This is pretty unusual.\nIs there a Steam Library on that drive that wasn't found?", "No Library found.", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        MessageBox.Show("You will now be asked for the location of the common folder. Please enter the path of the common folders location. Be very sure to write it like this:\n\nC:\\Program Files\\Steam\\steamapps\\common\n\nIf it's not written like this the program will NOT work correctly.\nBe very sure to get it right.", "CAUTION");
                        string alternative = Prompt.ShowDialog("Enter Path", "Enter Path  (C:\\ drive)");
                        if (Directory.Exists(alternative)) 
                        {
                            locs.Push(alternative);
                            tb_output.Text += (@"O - " + alternative + " found." + "\r\n");
                        }
                        else 
                        {
                            MessageBox.Show("The entered path was not found and has been discarded.");
                            tb_output.Text += (@"X - " + alternative + " NOT FOUND." + "\r\n");
                        }
                    }
                }
            }

            //test the rest
            foreach (string drive in drives) 
            {
                if (Directory.Exists(drive + @"SteamLibrary\steamapps\common"))
                {
                    locs.Push(drive + @"SteamLibrary\steamapps\common");
                    tb_output.Text += ("O - " + drive +@"SteamLibrary\steamapps\common found." + "\r\n");
                }
                else 
                {
                    tb_output.Text += ("X - " + drive + @"SteamLibrary\steamapps\common NOT FOUND." + "\r\n");
                    //yesno: does the drive have a steam library?
                    DialogResult dialogResult = MessageBox.Show("There was no Steam Library found on the " + drive + " drive.\nIs there a Steam Library on that drive?", "No Library found.", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        MessageBox.Show("You will now be asked for the location of the common folder. Please enter the path of the common folders location. Be very sure to write it like this:\n\nD:\\SteamLibrary\\steamapps\\common\n\nIf it's not written like this the program will NOT work correctly.\nBe very sure to get it right.", "CAUTION");
                        string alternative = Prompt.ShowDialog("Enter Path", "Enter Path (" + drive + " drive)");
                        if (Directory.Exists(alternative))
                        {
                            locs.Push(alternative);
                            tb_output.AppendText(@"O - " + alternative + " found." + "\r\n");
                        }
                        else
                        {
                            MessageBox.Show("The entered path was not found and has been discarded.");
                            tb_output.AppendText(@"X - " + alternative + " NOT FOUND." + "\r\n");
                        }
                    }
                }
            }
            //return all locations
            return locs;
        }
    }
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

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
            //list.Add("none");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace LosuLosu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool resultsFlag = false;
        int numberOfPeople = 0;
        List<string> listOfPeople = new List<string>();

        public void SetButtons(bool state)
        {
            btnRandomize.IsEnabled = state;
            btnRemoveAll.IsEnabled = state;
        }


        public void Refresh()
        {
            btnAdd.IsEnabled = false;
            btnRemove.IsEnabled = false;
            SetButtons(false);

            if (File.Exists("Teammates.txt"))
            {
                numberOfPeople = File.ReadLines("Teammates.txt").Count();

                if (numberOfPeople < 1)
                    SetButtons(false);

                else
                    SetButtons(true);

                labelCounter.Content = numberOfPeople;
                listOfPeople.Clear();
                
                    foreach (string line in File.ReadLines("Teammates.txt", Encoding.UTF8))
                        listOfPeople.Add(line);
            }

            else
            {
                listboxTeam.Items.Clear();
                numberOfPeople = 0;
                labelCounter.Content = numberOfPeople;
                listOfPeople.Clear();
            }

            if (numberOfPeople > 0 && File.Exists("Teammates.txt"))
            {
                listboxTeam.Items.Clear();

                foreach (string line in File.ReadLines("Teammates.txt", Encoding.UTF8))
                    listboxTeam.Items.Add(line);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            listboxTeam.SelectionMode = SelectionMode.Multiple;
            Refresh();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            txtbxPerson.Text = txtbxPerson.Text.Replace(" ", "");

            if (txtbxPerson.Text == "")
            {
                MessageBox.Show("Insert person to add.", "Nice try");
                return;
            }

            txtbxPerson.Text = txtbxPerson.Text.ToLower();

            if (!File.Exists("Teammates.txt"))
            {
                StreamWriter sw = File.CreateText("Teammates.txt");

                sw.WriteLine(txtbxPerson.Text);
                sw.Close();
            }
            else
            {
                foreach (string line in File.ReadLines("Teammates.txt", Encoding.UTF8))
                {
                    if (line.Equals(txtbxPerson.Text))
                    {
                        MessageBox.Show(txtbxPerson.Text + " is already on the list.", "Oh man");
                        txtbxPerson.Text = "";
                        return;
                    }
                }

                StreamWriter sw = File.AppendText("Teammates.txt");

                sw.WriteLine(txtbxPerson.Text);
                sw.Close();
            }

            txtbxPerson.Text = "";
            Refresh();
        }

        private void BtnRandomize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.Parse(txtbxTeamSize.Text) <= 0)
                {
                    MessageBox.Show("Insert real team size", "Very Funny");
                    return;
                }

                if (!resultsFlag)
                {
                    int teamSize = Int32.Parse(txtbxTeamSize.Text);
                    double temp = listOfPeople.Count() / teamSize;

                    double teamsNumber = Math.Ceiling(temp);

                    ResultWindow result1 = new ResultWindow(teamsNumber, teamSize, listOfPeople);
                    result1.Show();

                    //resultsFlag = true;
                }
            }
            catch
            {
                MessageBox.Show("Insert real team size", "Very Funny");
            }
        }

        private void BtnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("Teammates.txt"))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Remove all people?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    File.Delete("Teammates.txt");
                    //MessageBox.Show("All poeple removed", "BOOM");
                    Refresh();
                    return;
                }
                return;
            }

            MessageBox.Show("There is no one to remove!", "Are you kidding me?");
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(this.listboxTeam.SelectedIndex >= 0)
            {
                var newlist = listboxTeam.SelectedItems.Cast<string>().ToList();
                
                foreach(string s in newlist)
                    listboxTeam.Items.Remove(s);

                File.Delete("Teammates.txt");
                StreamWriter sw = File.CreateText("Teammates.txt");

                foreach(var person in listboxTeam.Items)
                    sw.WriteLine(person);

                sw.Close();
                Refresh();

                return;
            }

            MessageBox.Show("Select person to remove.");
        }

        private void listboxTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxTeam.SelectedIndex != -1)
                btnRemove.IsEnabled = true;
            else
                btnRemove.IsEnabled = false;
        }

        private void txtbxPerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtbxPerson.Text.Length >= 0)
                btnAdd.IsEnabled = true;
        }

        private void txtbxPerson_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtbxPerson.Text.Length == 0 && (e.Key == Key.Back || e.Key == Key.Delete))
                btnAdd.IsEnabled = false;
        }


        //private void BtnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (File.Exists("Teammates.txt"))
        //        Process.Start("notepad.exe", "Teammates.txt");
        //    else
        //        MessageBox.Show("There is nothing to edit.", ":/");
        //}
    }
    }


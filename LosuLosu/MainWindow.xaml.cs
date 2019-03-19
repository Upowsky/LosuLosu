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
        int numberOfPeople = 0;
        List<string> listOfPeople = new List<string>();

        public void Refresh()
        {
            if (File.Exists("Teammates.txt"))
            {
                numberOfPeople = File.ReadLines("Teammates.txt").Count();
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
            Refresh();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxPerson.Text == "")
                return;

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

                int teamSize = Int32.Parse(txtbxTeamSize.Text);
                double temp = listOfPeople.Count() / teamSize;

                double teamsNumber = Math.Ceiling(temp);

                ResultWindow result1 = new ResultWindow(teamsNumber, teamSize, listOfPeople);
                result1.Show();
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
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    File.Delete("Teammates.txt");
                    MessageBox.Show("All poeple removed", "BOOM");
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
                this.listboxTeam.Items.RemoveAt(this.listboxTeam.SelectedIndex);

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

        //private void BtnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (File.Exists("Teammates.txt"))
        //        Process.Start("notepad.exe", "Teammates.txt");
        //    else
        //        MessageBox.Show("There is nothing to edit.", ":/");
        //}
    }
    }


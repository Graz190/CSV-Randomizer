using CSV_Randomizer.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSV_Randomizer.Views
{
    /// <summary>
    /// Interaktionslogik für RandomizerHome.xaml
    /// </summary>
    public partial class RandomizerHome : Page
    {
        private readonly BackgroundWorker worker1 = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        Randomizer rand;
        List<string> printList=new List<string>();
        public RandomizerHome()
        {
            InitializeComponent();
            this.DataContext = this;
            worker1.DoWork += Worker1_DoWork;
            worker1.RunWorkerCompleted += Worker1_RunWorkerCompleted;
            worker2.DoWork += Worker2_DoWork;
            worker2.RunWorkerCompleted += Worker2_RunWorkerCompleted;
            rand = new Randomizer(this);
        }
        /// <summary>
        /// Method for PrintButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if(ListBox.Items.IsEmpty) { showMessage(ColorText.error, "Kein Inhalt vorhanden"); return; }
            if (!worker2.IsBusy)
            {
                worker2.RunWorkerAsync();
            }

        }
        /// <summary>
        /// Method for StartButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(openFilePathBox.Text))
            {
                showMessage(ColorText.error, "Bitte einen gütigen Dateipfad angeben.");
                return;
            }  
            PropertySetting.Save_Setting(Settingname.FilePath, openFilePathBox.Text);
            if (!worker1.IsBusy)
            {
                worker1.RunWorkerAsync();
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ListBox.Items.Clear();
            openFilePathBox.Text = "";
            SliderValue.Value = 10;
            showMessage(ColorText.success, "Programm erfolgreich zurückgesetzt");
        }
        public void showMessage(Brush color, string message)
        {
            this.MessageInfo.Text = message;
            this.MessageInfo.TextAlignment = TextAlignment.Center;
            this.MessageInfo.Foreground = color;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Control ctrl))
            {
                return;
            }
            String name = ctrl.Name;
            if (name == "Einstellung")
            {
            }
            else if (name == "Credit")
            {
                MessageBox.Show("CSV-Randomizer Version 0.2.1\n\nMade with Love by Christian Fagherazzi", "Credits");
            }
        }
        private void Worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            showMessage(ColorText.success, "Programm hat " + ListBox.Items.Count + " zufällig ausgewählt");
        }
        private void Worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int percentValue=0;
            Dispatcher.Invoke(() =>
            {
                ListBox.Items.Clear();
                showMessage(ColorText.loading, "Bitte warten Programm wählt zufällig eine Zahl aus");
            });
            percentValue = int.Parse(PropertySetting.Read_Setting(Settingname.SliderValue));
            printList.Clear();
            printList = rand.chooseRandom(percentValue);
            foreach (String temp in printList)
            {
                Dispatcher.Invoke(() =>
                {
                    ListBox.Items.Add(temp);
                });
            }

        }
        private void Worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            showMessage(ColorText.success, "Die CSV wurde erfolgreich gespeichert");
        }
        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.Filter = "Csv Datei (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == true)
            {
                rand.writeCSV(saveFileDialog1.FileName, printList);
            }

        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "CSV Dateien (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true) openFilePathBox.Text = openFileDialog.FileName;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PropertySetting.Save_Setting(Settingname.SliderValue,SliderValue.Value.ToString());
        }
    }
}

using CSV_Randomizer.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace CSV_Randomizer.Views
{
    /// <summary>
    /// Interaktionslogik für RandomizerHome.xaml
    /// </summary>
    public partial class RandomizerHome : Page
    {
        private readonly BackgroundWorker worker1 = new BackgroundWorker();
        Randomizer rand;
        List<String> printList=new List<String>();
        public RandomizerHome()
        {
            InitializeComponent();
            this.DataContext = this;
            worker1.DoWork += Worker1_DoWork;
            worker1.RunWorkerCompleted += Worker1_RunWorkerCompleted;
            rand = new Randomizer(this);
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (!worker1.IsBusy)
            {
                //worker1.RunWorkerAsync();
            }
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            PropertySetting.Save_Setting(Settingname.FilePath, openFilePathBox.Text);
            if (!worker1.IsBusy)
            {
                worker1.RunWorkerAsync();
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ListBox.Items.Clear();
        }
        public void showMessage(Brush color, string message)
        {
            this.MessageInfo.Text = message;
            this.MessageInfo.TextAlignment = TextAlignment.Center;
            this.MessageInfo.Foreground = color;
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

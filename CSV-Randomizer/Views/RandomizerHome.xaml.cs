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
            if(ListBox.Items.IsEmpty) { showMessage(ColorText.error, Properties.Resources.errorEmpty); return; }
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
                showMessage(ColorText.error, Properties.Resources.invalidPath);
                return;
            }  
            PropertySetting.Save_Setting(Settingname.FilePath, openFilePathBox.Text);
            if (!worker1.IsBusy)
            {
                worker1.RunWorkerAsync();
            }
        }
        /// <summary>
        /// Method for ResetButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ListBox.Items.Clear();
            openFilePathBox.Text = "";
            SliderValue.Value = 10;
            showMessage(ColorText.success, Properties.Resources.reset);
        }
        /// <summary>
        /// Display given Message with given font color on Mainview. Use ColorText for uniform colorcodes 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="message"></param>
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
            if (name == "Sprache")
            {
            }
            else if (name == "Credit")
            {
                MessageBox.Show("CSV-Randomizer Version 1.1.0\n\nMade with Love by Christian Fagherazzi", "Credits");
            }
        }
        /// <summary>
        /// Completed Method for Backgroundworker worker 1 displayed only a Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            showMessage(ColorText.success, Properties.Resources.successProgram1+ ListBox.Items.Count + Properties.Resources.successProgram2);
        }
        /// <summary>
        /// DoWork Method for Backgroundworker worker 1. It cleared previous ListBoxitems and fill it with new values from Randomizer.chooseRandom method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int percentValue=0;
            Dispatcher.Invoke(() =>
            {
                ListBox.Items.Clear();
                showMessage(ColorText.loading, Properties.Resources.loading);
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
        /// <summary>
        /// Completed Method for Backgroundworker worker 2 displayed only a Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            showMessage(ColorText.success, Properties.Resources.successCsv);
        }
        /// <summary>
        /// DoWork Method for Backgroundworker worker 1. It prints value from ListBox into a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Title = Properties.Resources.titleSaveFile;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.Filter = "Csv Datei (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == true)
            {
                rand.writeCSV(saveFileDialog1.FileName, printList);
            }

        }
        /// <summary>
        /// Open Filedialog and save result into openFilePathBox.Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "CSV Dateien (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true) openFilePathBox.Text = openFileDialog.FileName;

        }
        /// <summary>
        /// save slider setting every time, when their is a TextChange in the SlideTextbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PropertySetting.Save_Setting(Settingname.SliderValue,SliderValue.Value.ToString());
        }

    }
}

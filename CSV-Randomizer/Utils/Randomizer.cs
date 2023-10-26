using CSV_Randomizer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace CSV_Randomizer.Utils
{
    internal class Randomizer
    {
        RandomizerHome RandomizerHome { get; set; }
        static Random rnd = new Random();
        public Randomizer(RandomizerHome home) {
            RandomizerHome = home;
        }

        private List<String> loadCSV()
        {
            List<String> list = new List<String>();
            try { 
            var reader = new StreamReader(File.OpenRead(PropertySetting.Read_Setting(Settingname.FilePath)));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                list.Add(values[0]);
            }
            }catch(Exception ex)
            {
                RandomizerHome.Dispatcher.Invoke(() =>
                {
                    RandomizerHome.showMessage(ColorText.error, ex.Message);
                });
            }
            return list;
        }
        /// <summary>
        /// Main Method TODO
        /// </summary>
        /// <param name="percent">choosen percent</param>
        /// <returns>sample as a String list</returns>
        public List<String> chooseRandom(int percent)
        {
            double choosedPercent = (double)percent / 100;
            List<String> csvList = loadCSV();
            List<String> randomList = new List<String>();
            int listSize = csvList.Count;
            int cycleCount = (int)Math.Round((double)(choosedPercent * listSize - 1));

            for (int i = 0; i <= cycleCount; i++)
            {
                int r = 0;
                while (r == 0)
                    r = rnd.Next(csvList.Count - 1);
                randomList.Add(csvList[r]);
                csvList.Remove(csvList[r]);
            }
            return randomList;
        }
        /// <summary>
        /// Writes sample in a CSV-file
        /// </summary>
        /// <param name="path">Outputfolder Path</param>
        /// <param name="listString">Sample list</param>
        public void writeCSV(String path, List<String> listString)
        {
            try
            {
                using (var file = File.CreateText(path))
                {
                    for (int i=0;i<listString.Count;i++)
                    {
                        file.Write(listString[i]);
                        if(i<listString.Count)file.Write(',');

                        file.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                RandomizerHome.Dispatcher.Invoke(() =>
                {
                    RandomizerHome.showMessage(ColorText.error, ex.Message);
                });
            }
        }
    }
}

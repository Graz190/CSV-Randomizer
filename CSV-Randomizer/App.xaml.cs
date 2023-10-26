using System.Globalization;
using System.Windows;

namespace CSV_Randomizer
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (!ci.Name.Equals("en")&&!ci.Name.Equals("de-DE")) {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }
        }
    }
}

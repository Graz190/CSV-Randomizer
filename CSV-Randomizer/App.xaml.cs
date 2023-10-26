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
            if (!ci.Name.Equals("en") && !ci.Name.StartsWith("de-") && !ci.Name.StartsWith("en-"))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(ci.Name);
            }
        }
    }
}

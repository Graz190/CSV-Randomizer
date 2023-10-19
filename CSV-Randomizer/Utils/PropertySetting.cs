using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Randomizer.Utils
{
    internal class PropertySetting
    {
        public static void Save_Setting(string setting_Name, string setting_Value)
        {
            if (setting_Value is null)
            {
                throw new ArgumentNullException(nameof(setting_Value));
            }

            var property_name = setting_Name;

            SettingsProperty prop;
            if (Properties.Settings.Default.Properties[property_name] != null)
            {
                _ = Properties.Settings.Default.Properties[property_name];
            }
            else
            {
                prop = new SettingsProperty(property_name)
                {
                    PropertyType = typeof(string)
                };
                Properties.Settings.Default.Properties.Add(prop);
                Properties.Settings.Default.Save();
            }
            Properties.Settings.Default.Properties[property_name].DefaultValue = setting_Value;

            Properties.Settings.Default.Save();
        }
        public static string Read_Setting(string setting_Name)
        {
            string sResult = "";
            if (Properties.Settings.Default.Properties[setting_Name] != null)
            {
                sResult = Properties.Settings.Default.Properties[setting_Name].DefaultValue.ToString();
            }
            if (sResult == "NaN") sResult = "0";

            return sResult;
        }
    }
}

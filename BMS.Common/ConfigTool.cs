using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Common
{
    public class ConfigTool
    {
        public static string GetDBConnectionString(string connName)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[connName].ConnectionString.ToString();
        }

        public static string GetAppsetting(string Key)
        {
            string AppSetting = string.Empty;
            AppSetting = System.Configuration.ConfigurationManager.AppSettings[Key];
            return AppSetting;
        }
    }
}

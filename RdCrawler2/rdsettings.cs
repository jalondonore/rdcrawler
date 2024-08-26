using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RdCrawler2
{
    class rdsettings
    {
        public static bool redirect;

        public void loadsettings(bool htmlpagesoption, bool fullsitecrawloption, bool redirectsoption, bool getexurlsoption, bool rendermodeoption, bool ignoreresourcesoption)
        {
            AppSettingsReader crawlersetings = new AppSettingsReader();
            htmlpagesoption = (bool)crawlersetings.GetValue("htmlpages", typeof(bool));
            fullsitecrawloption = (bool)crawlersetings.GetValue("fullsitecrawl", typeof(bool));
            redirectsoption = (bool)crawlersetings.GetValue("redirects", typeof(bool));
            getexurlsoption = (bool)crawlersetings.GetValue("getexurls", typeof(bool));
            rendermodeoption = (bool)crawlersetings.GetValue("rendermode", typeof(bool));
            ignoreresourcesoption = (bool)crawlersetings.GetValue("ignoreresources", typeof(bool));
        }

        public void savesettings(bool htmlpagesoption, bool fullsitecrawloption, bool redirectsoption, bool getexurlsoption, bool rendermodeoption, bool ignoreresourcesoption)
        {
            Properties.Settings.Default["htmlpages"] = htmlpagesoption;
            Properties.Settings.Default["fullsitecrawl"] = fullsitecrawloption;
            Properties.Settings.Default["redirects"] = redirectsoption;
            Properties.Settings.Default["getexurls"] = getexurlsoption;
            Properties.Settings.Default["rendermode"] = rendermodeoption;
            Properties.Settings.Default["ignoreresources"] = ignoreresourcesoption;
            
            Properties.Settings.Default.Save();
        }

        public static void setvalue (string key, bool value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            config.AppSettings.Settings.Add(key, value.ToString());
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string readvalues(string key, string pdefault)
        {
            string retorno = ConfigurationManager.AppSettings[key];
            if (retorno == null) { retorno = pdefault; }
            return retorno;
        }








    }
}

using GoogleSheetImportWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace GoogleSheetImportWeb.Service
{
    public class UserConfig
    {
        public static ConfigModel ReadConfig()
        {
            ConfigModel model = new ConfigModel()
            {
                /* google config */
                GoogleInfo = new GoogleInfo()
                {
                    ClientID = ConfigurationManager.AppSettings["GoogleClientID"],
                    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"],
                    Url = ConfigurationManager.AppSettings["GoogleSheetUrl"],
                    SheetName = ConfigurationManager.AppSettings["GoogleSheetName"],
                },

                /* sql config */
                SqlInfo = new SqlInfo()
                {
                    Server = ConfigurationManager.AppSettings["SQLServer"],
                    Username = ConfigurationManager.AppSettings["SQLUsername"],
                    Password = ConfigurationManager.AppSettings["SQLPassword"],
                    DatabaseName = ConfigurationManager.AppSettings["DatabaseName"],
                    TableName = ConfigurationManager.AppSettings["TableName"],
                }
            };
            return model;
        }

        /* delete token for change google acc */
        public static bool DeleteToken()
        {
            try
            {
                string dir = HttpContext.Current.Server.MapPath("~/token.json");
                if (Directory.Exists(dir))
                    Directory.Delete(dir, true);
                return true;
            }
            catch (Exception ex) { }
            return false;
        }
    }
}
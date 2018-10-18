using GoogleSheetImportWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                GoogleInfo = new GoogleInfo()
                {
                    ClientID = ConfigurationManager.AppSettings["GoogleClientID"],
                    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"],
                    Url = ConfigurationManager.AppSettings["GoogleSheetUrl"],
                    SheetName = ConfigurationManager.AppSettings["GoogleSheetName"],
                },
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
    }
}
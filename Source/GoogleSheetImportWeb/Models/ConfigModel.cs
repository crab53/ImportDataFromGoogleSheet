using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleSheetImportWeb.Models
{
    public class ConfigModel
    {
        public GoogleInfo GoogleInfo { get; set; }
        public SqlInfo SqlInfo { get; set; }

        public ConfigModel()
        {
            GoogleInfo = new GoogleInfo();
            SqlInfo = new SqlInfo();
        }
    }

    public class GoogleInfo
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string Url { get; set; }
        public string SheetName { get; set; }

        public GoogleInfo()
        {
            ClientID = "";
            ClientSecret = "";
            Url = "";
            SheetName = "";
        }
    }

    public class SqlInfo
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }

        public SqlInfo()
        {
            Server = "";
            Username = "";
            Password = "";
            DatabaseName = "";
            TableName = "";
        }
    }
}
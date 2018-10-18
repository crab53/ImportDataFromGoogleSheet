using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;

namespace GoogleSheetImportWeb.Models
{
    public class ConfigXMLModel
    {
        [Required(ErrorMessage = "Google Sheet URL is required")]
        public string GoogleSheetUrl { get; set; }
        [Required(ErrorMessage = "Google Sheet Name is required")]
        public string GoogleSheetName { get; set; }
        [Required(ErrorMessage = "SQL Server is required")]
        public string SQLServer { get; set; }
        [Required(ErrorMessage = "SQL Username is required")]
        public string SQLUsername { get; set; }
        [Required(ErrorMessage = "SQL Password is required")]
        public string SQLPassword { get; set; }
        [Required(ErrorMessage = "Database Name is required")]
        public string DatabaseName { get; set; }
        [Required(ErrorMessage = "Table Name is required")]
        public string TableName { get; set; }
        [Required(ErrorMessage = "Google Client ID is required")]
        public string GoogleClientID { get; set; }
        [Required(ErrorMessage = "Google Client Secret is required")]
        public string GoogleClientSecret { get; set; }
    }
}
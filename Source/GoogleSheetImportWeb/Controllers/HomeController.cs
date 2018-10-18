using GoogleSheetImportWeb.Models;
using GoogleSheetImportWeb.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace GoogleSheetImportWeb.Controllers
{


    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ConfigXMLModel()
            {
                /* get config from web.config */
                GoogleClientID = ConfigurationManager.AppSettings["GoogleClientID"],
                GoogleClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"],
                GoogleSheetUrl = ConfigurationManager.AppSettings["GoogleSheetUrl"],
                GoogleSheetName = ConfigurationManager.AppSettings["GoogleSheetName"],
                SQLServer = ConfigurationManager.AppSettings["SQLServer"],
                SQLUsername = ConfigurationManager.AppSettings["SQLUsername"],
                SQLPassword = ConfigurationManager.AppSettings["SQLPassword"],
                DatabaseName = ConfigurationManager.AppSettings["DatabaseName"],
                TableName = ConfigurationManager.AppSettings["TableName"],
            });
        }

        [HttpPost]
        public ActionResult Config(ConfigXMLModel model)
        {
            try
            {
                //==== Validation
                if (string.IsNullOrEmpty(model.GoogleSheetUrl))
                {
                    ModelState.AddModelError("GoogleSheetUrl", "Google Sheet URL is required");
                }
                if (string.IsNullOrEmpty(model.GoogleSheetName))
                {
                    ModelState.AddModelError("GoogleSheetName", "Google Sheet Name is required");
                }
                if (string.IsNullOrEmpty(model.SQLServer))
                {
                    ModelState.AddModelError("SQLServer", "SQL Server is required");
                }
                if (string.IsNullOrEmpty(model.SQLUsername))
                {
                    ModelState.AddModelError("SQLUsername", "SQL Username is required");
                }
                if (string.IsNullOrEmpty(model.SQLPassword))
                {
                    ModelState.AddModelError("SQLPassword", "SQL Password is required");
                }
                if (string.IsNullOrEmpty(model.DatabaseName))
                {
                    ModelState.AddModelError("DatabaseName", "Database Name is required");
                }
                if (string.IsNullOrEmpty(model.TableName))
                {
                    ModelState.AddModelError("TableName", "Table Name is required");
                }
                if (string.IsNullOrEmpty(model.GoogleClientID))
                {
                    ModelState.AddModelError("GoogleClientID", "Google Client ID is required");
                }
                if (string.IsNullOrEmpty(model.GoogleClientSecret))
                {
                    ModelState.AddModelError("GoogleClientSecret", "Google Client Secret is required");
                }

                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                /* Save config  */
                NameValueCollection nameValueCollection = new NameValueCollection();
                nameValueCollection.Add("GoogleClientID", model.GoogleClientID);
                nameValueCollection.Add("GoogleClientSecret", model.GoogleClientSecret);
                nameValueCollection.Add("GoogleSheetUrl", model.GoogleSheetUrl);
                nameValueCollection.Add("GoogleSheetName", model.GoogleSheetName);
                nameValueCollection.Add("SQLServer", model.SQLServer);
                nameValueCollection.Add("SQLUsername", model.SQLUsername);
                nameValueCollection.Add("SQLPassword", model.SQLPassword);
                nameValueCollection.Add("DatabaseName", model.DatabaseName);
                nameValueCollection.Add("TableName", model.TableName);
                XmlDocument doc = new XmlDocument();
                var fileName = HttpContext.Server.MapPath("~/Web.config");
                doc.Load(fileName);
                var _appSettings = doc.GetElementsByTagName("appSettings")[0].SelectNodes("add");
                foreach (var formEle in nameValueCollection.AllKeys)
                {
                    foreach (XmlNode node in _appSettings)
                    {
                        if (node.Attributes["key"].Value.ToLower() == formEle.ToLower())
                        {
                            node.Attributes["value"].Value = nameValueCollection[formEle];
                        }
                    }
                }
                doc.Save(fileName); /* save to web.config */
                
                /* check google sheet error */
                GoogleService googleService = new GoogleService();
                if (googleService.GetSheet() == null)
                {
                    ModelState.AddModelError("ErrorGoogle", "Unable to connect to google service.");
                }

                /* check sql error */
                DatabaseService databaseService = new DatabaseService();
                if (!databaseService.CheckConnectToSQLServer())
                {
                    ModelState.AddModelError("ErrorDatabase", "Unable to connect to database.");
                }
                else if (databaseService.GetColumns().Count == 0)
                {
                    ModelState.AddModelError("ErrorTable", "Unable to find table name in database.");
                }

                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }
            }
            catch (Exception ex) { }
            return RedirectToAction("Index", "Mapping");
        }

        /* delete account when want to change account */
        public bool ChangeAccount()
        {
            var result = UserConfig.DeleteToken();
            return result;
        }
    }
}
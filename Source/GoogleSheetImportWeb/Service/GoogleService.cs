using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using GoogleSheetImportWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace GoogleSheetImportWeb.Service
{
    public class GoogleService
    {
        ConfigModel config = null;
        string url = "";
        string sheetName = "";

        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly, SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart"; /* google app */

        public GoogleService()
        {
            config = UserConfig.ReadConfig();

            url = config.GoogleInfo.Url;
            sheetName = config.GoogleInfo.SheetName;
        }

        public IList<IList<Object>> GetSheet()
        {
            try
            {
                /* get credential */
                string fileName = HttpContext.Current.Server.MapPath("~/token.json");
                UserCredential credential = credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = config.GoogleInfo.ClientID,
                        ClientSecret = config.GoogleInfo.ClientSecret,
                    },
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(fileName, true)).Result;

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                var request = service.Spreadsheets.Values.Get(GetSpreadSheetId(url), sheetName);
                ValueRange response = request.Execute();

                return response.Values;
            }
            catch (Exception ex) { }
            return null;
        }

        /* get sheet ID from sheet url */
        private string GetSpreadSheetId(string url)
        {
            Regex regex = new Regex("/spreadsheets/d/([a-zA-Z0-9-_]+)");
            Match match = regex.Match(url);
            return match.Groups[1].Value;
        }
    }
}
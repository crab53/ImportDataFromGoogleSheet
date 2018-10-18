using GoogleSheetImportWeb.Models;
using GoogleSheetImportWeb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GoogleSheetImportWeb.Controllers
{
    public class MappingController : Controller
    {
        private DatabaseService databaseService = new DatabaseService();
        // GET: Mapping
        public ActionResult Index()
        {
            GoogleService googleService = new GoogleService();

            MappingModel model = new MappingModel()
            {
                ListData = googleService.GetSheet(),
                ListColumn = databaseService.GetColumns(),
            };

            if (model.ListData == null || !model.ListData.Any() || model.ListColumn == null || !model.ListColumn.Any())
            {
                ModelState.AddModelError("Error", "Please check your config again");
                return View(model);
            } 
            
            if (model.ListData[0] != null && model.ListData[0].Any())
            {
                var lstHeader = model.ListData[0];
                int index = 0;
                foreach (var data in lstHeader)
                {
                    model.ListMapping.Add(new DataMapModel()
                    {
                        Header = data.ToString(),
                        Index = index
                    });
                    index++;
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveDB(MappingModel model)
        {
            try
            {
                //==== Check column name is null or empty
                var lstColNull = model.ListMapping.Where(w => string.IsNullOrEmpty(w.ColumnName)).ToList();
                if (lstColNull != null && lstColNull.Any())
                {
                    foreach(var itm in lstColNull)
                    {
                        ModelState.AddModelError("ListMapping[" + itm.Index + "].ColumnName", "Please choose Column Name");
                    }
                }

                //==== Check duplicate column name
                var lstColSelected = model.ListMapping.Where(w => !string.IsNullOrEmpty(w.ColumnName)).ToList();
                var lstColSelectedGroup = lstColSelected.GroupBy(g => g.ColumnName).Where(w => w.Count() > 1).ToList();

                foreach(var itm in lstColSelectedGroup)
                {
                    foreach (var item in itm)
                    {
                        ModelState.AddModelError("ListMapping[" + item.Index + "].ColumnName", "Please choose Other Column Name");
                    }
                }

                if (!ModelState.IsValid)
                {
                    GoogleService googleService = new GoogleService();
                    
                    model.ListData = googleService.GetSheet();
                    model.ListColumn = databaseService.GetColumns();
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return View("Index", model);
                }

                var result = databaseService.InsertData(model.ListMapping);
                if (result)
                {
                    return View("Result");
                }
                else
                {
                    ModelState.AddModelError("Error", "Import data failed");
                    GoogleService googleService = new GoogleService();

                    model.ListData = googleService.GetSheet();
                    model.ListColumn = databaseService.GetColumns();

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                GoogleService googleService = new GoogleService();

                model.ListData = googleService.GetSheet();
                model.ListColumn = databaseService.GetColumns();

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View("Index", model);
            }
        }
    }
}
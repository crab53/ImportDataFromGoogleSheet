using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleSheetImportWeb.Models
{
    public class MappingModel
    {
        public IList<IList<object>> ListData { get; set; }
        public List<DataColumnModel> ListColumn { get; set; }
        public List<DataMapModel> ListMapping { get; set; }
        public MappingModel()
        {
            ListMapping = new List<DataMapModel>();
        }
    }
    public class DataMapModel
    {
        public int Index { get; set; }
        public string Header { get; set; }
        public string ColumnName { get; set; }
        public string ColumnDataType { get; set; }
    }
}
using GoogleSheetImportWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GoogleSheetImportWeb.Service
{
    public class DatabaseService
    {
        string connectionString = "";
        ConfigModel config = null;

        /* init connection string */
        public DatabaseService()
        {
            /* init connection string */
            config = UserConfig.ReadConfig();

            connectionString = string.Format("Data Source={0};Initial Catalog={1}; User Id={2}; Password={3}",
                config.SqlInfo.Server,
                config.SqlInfo.DatabaseName,
                config.SqlInfo.Username,
                config.SqlInfo.Password);
        }

        /* get column name & type of tbl from db */
        public List<DataColumnModel> GetColumns()
        {
            List<DataColumnModel> listColumns = new List<DataColumnModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    /* get column name from db */
                    command.CommandText = string.Format("SELECT c.COLUMN_NAME, c.DATA_TYPE FROM information_schema.columns c WHERE c.table_schema = 'dbo' AND c.table_name = '{0}' ORDER BY c.ORDINAL_POSITION", config.SqlInfo.TableName);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listColumns.Add(new DataColumnModel()
                            {
                                ColumnName = reader.GetString(0),
                                DataType = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return listColumns.OrderBy(o => o.ColumnName).ToList();
        }

        /* Insert data to sql */
        public bool InsertData(List<DataMapModel> mappings)
        {
            mappings = mappings.Where(o => !string.IsNullOrEmpty(o.ColumnName)).ToList();
            bool result = false;
            var columns = this.GetColumns();

            /* get data from google sheet */
            GoogleService googleService = new GoogleService();
            var listData = googleService.GetSheet();
            if (listData.Count > 0)
            {
                var headers = listData[0].Cast<string>().ToList();
                foreach (var header in mappings)
                {
                    header.Index = headers.IndexOf(header.Header);
                    header.ColumnDataType = columns.Where(o => o.ColumnName == header.ColumnName).Select(o => o.DataType).FirstOrDefault();
                }

                /* make command string */
                string sqlComm = string.Format("INSERT INTO {0} ", config.SqlInfo.TableName);
                for (int i = 0; i < listData.Count; i++)
                {
                    for (int j = 0; j < listData[i].Count; j++)
                    {
                        var map = mappings.Where(o => o.Header == listData[0][j].ToString()).FirstOrDefault();
                        if (map != null)
                        {
                            if (j == 0) sqlComm += "(";
                            if (i == 0)
                            {
                                sqlComm += map.ColumnName;
                                if (j == listData[i].Count - 1)
                                    sqlComm += ") VALUES ";
                                else
                                    sqlComm += ",";
                            }
                            else
                            {
                                sqlComm += GetValue(listData[i][map.Index], map.ColumnDataType);
                                if (j == listData[i].Count - 1)
                                    sqlComm += ")";
                                else
                                    sqlComm += ",";
                            }
                        }
                    }
                    if (i != 0 && i != listData.Count - 1)
                        sqlComm += ",";
                }

                /* execute sql command */
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlComm;
                    connection.Open();
                    result = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return result;
        }

        /* Check connect to SQL */
        public bool CheckConnectToSQLServer()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        /* format data to sql data type */
        private string GetValue(object value, string dataType)
        {
            string valueStr = value.ToString().Replace("'", "''");
            switch (dataType)
            {
                case "nvarchar": return string.Format("N'{0}'", valueStr);
                case "datetime":
                case "text": return string.Format("'{0}'", valueStr);
                case "bit": return valueStr.ToLower() == "true" ? "1" : "0";
                default: return value.ToString();
            }
        }
    }
}
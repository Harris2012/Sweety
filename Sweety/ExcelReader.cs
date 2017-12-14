using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Sweety
{
    class ExcelReader
    {
        private static readonly DateTime DefaultDay = new DateTime(1900, 1, 1);

        public static List<T> ReadEntityList<T>(string inputFilePath, string tableName) where T : class, new()
        {
            var table = GetExcelTableByOleDB(inputFilePath, tableName);

            if (table == null && table.Rows.Count == 0)
            {
                return null;
            }

            List<T> entityList = ToEntityList<T>(table);

            return entityList;
        }

        private static List<T> ToEntityList<T>(DataTable table) where T : class, new()
        {
            var type = typeof(T);

            //Excel所有列
            List<string> columnNames = new List<string>(table.Columns.Count);
            var firstRow = table.Rows[0];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                columnNames.Add(firstRow[i].ToString());
            }

            //为属性确定列
            List<ColumnInfo> columnInfoList = new List<ColumnInfo>();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (!property.IsDefined(typeof(ExcelColumnAttribute), false))
                {
                    continue;
                }

                var columnAttribute = (ExcelColumnAttribute)property.GetCustomAttributes(typeof(ExcelColumnAttribute), false)[0];
                var index = columnNames.IndexOf(columnAttribute.Name);
                if (index < 0)
                {
                    continue;
                }

                columnAttribute.ColumnIndexFromExcel = index;
                columnInfoList.Add(new ColumnInfo { PropertyInfo = property, ColumnAttribute = columnAttribute });
            }

            //读取Excel
            List<T> entityList = new List<T>();
            for (int i = 1; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];

                var entity = new T();

                foreach (var columnInfo in columnInfoList)
                {
                    var property = columnInfo.PropertyInfo;
                    var value = row[columnInfo.ColumnAttribute.ColumnIndexFromExcel].ToString();
                    try
                    {
                        switch (columnInfo.PropertyInfo.PropertyType.ToString())
                        {
                            case "System.Int32":
                                {
                                    int intValue = 0;
                                    if (int.TryParse(value, out intValue))
                                    {
                                        property.SetValue(entity, intValue, null);
                                    }
                                }
                                break;
                            case "System.Int64":
                                {
                                    long longValue = 0;
                                    if (long.TryParse(value, out longValue))
                                    {
                                        property.SetValue(entity, longValue, null);
                                    }
                                }
                                break;
                            case "System.Single":
                                {
                                    float floatValue = 0;
                                    if (float.TryParse(value, out floatValue))
                                    {
                                        property.SetValue(entity, floatValue, null);
                                    }
                                }
                                break;
                            case "System.Double":
                                {
                                    double doubleValue = 0;
                                    if (double.TryParse(value, out doubleValue))
                                    {
                                        property.SetValue(entity, doubleValue, null);
                                    }
                                }
                                break;
                            case "System.Decimal":
                                {
                                    decimal decimalValue = 0;
                                    if (decimal.TryParse(value, out decimalValue))
                                    {
                                        property.SetValue(entity, decimalValue, null);
                                    }
                                }
                                break;
                            case "System.DateTime":
                                {
                                    int intValue = 0;
                                    DateTime dateTimeValue = DateTime.MinValue;
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        if (DateTime.TryParse(value, out dateTimeValue))
                                        {
                                            property.SetValue(entity, dateTimeValue, null);
                                        }
                                        else if (int.TryParse(value, out intValue))
                                        {
                                            dateTimeValue = DefaultDay.AddDays(intValue);
                                            property.SetValue(entity, dateTimeValue, null);
                                        }
                                    }
                                }
                                break;
                            case "System.String":
                                {
                                    property.SetValue(entity, value, null);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                entityList.Add(entity);
            }

            return entityList;
        }

        /// <summary>
        /// 从excel中读取数据
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="tableName">表名，不需要以$结尾</param>
        /// <returns></returns>
        private static DataTable GetExcelTableByOleDB(string inputFilePath, string tableName)
        {
            DataTable table = new DataTable();

            tableName = tableName + "$";

            using (OleDbConnection conn = GetConnection(inputFilePath))
            {
                conn.Open();

                var schema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, tableName, "TABLE" });
                if (schema.Rows.Count == 0)
                {
                    return null;
                }

                DataSet ds = new DataSet();

                string sql = string.Format("select * from [{0}]", tableName);

                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);

                adapter.Fill(ds);

                table = ds.Tables[0];
            }

            return table;
        }

        //获取Excel中所有Sheet表的信息
        //System.Data.DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
        //获取Excel的第一个Sheet表名
        //string tableName_0 = schemaTable.Rows[0][2].ToString().Trim();

        private static OleDbConnection GetConnection(string inputFilePath)
        {
            if (inputFilePath.EndsWith(".xls"))
            {
                return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + inputFilePath + ";" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"");
            }
            else if (inputFilePath.EndsWith(".xlsx"))
            {
                return new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + inputFilePath + ";" + "Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\"");
            }

            return null;
        }
    }
}

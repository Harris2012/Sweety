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
        public static List<T> ReadEntityList<T>(string inputFilePath, string tableName)
        {
            var table = GetExcelTableByOleDB(inputFilePath, tableName);

            if (table == null)
            {
                return null;
            }

            List<T> entityList = new List<T>();

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

                string sql = string.Format("select * from [{0}$]", tableName);

                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);

                adapter.Fill(ds);

                table = ds.Tables[tableName];
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

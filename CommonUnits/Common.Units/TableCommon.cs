using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Units
{
     /// <summary>
    /// Author;Gavin
    /// Create Date: 2016-08-23
    /// Description:DataTable类
    /// </summary>
    public class TableCommon
    {
        /// <summary>
        /// DataTable转Json字符
        /// </summary>
        /// <param name="dtTable">数据源</param>
        /// <param name="strJsonName">json名称</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dtTable, string strJsonName)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            try
            {       
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    JsonSerializer ser = new JsonSerializer();
                    jw.WriteStartObject();
                    jw.WritePropertyName(strJsonName);
                    jw.WriteStartArray();
                    foreach (DataRow dr in dtTable.Rows)
                    {
                        jw.WriteStartObject();

                        foreach (DataColumn dc in dtTable.Columns)
                        {
                            jw.WritePropertyName(dc.ColumnName);
                            ser.Serialize(jw, dr[dc].ToString());
                        }

                        jw.WriteEndObject();
                    }
                    jw.WriteEndArray();
                    jw.WriteEndObject();

                    sw.Close();
                    jw.Close();
                }
            }
            catch (Exception ex)
            {
                Common.Units.Error.WriteErrorXml(dtTable + "转json出错:" + ex.Message);
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 给Datatable分页
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageSize">每页数量</param>
        /// <returns></returns>
        public static DataTable GetPageTable(DataTable dt, int PageIndex, int PageSize)
        {
            DataTable newdt = dt.Copy();
            try
            {
                if (PageIndex == 0)
                    return dt;
          
                newdt.Clear();
                int rowbegin = (PageIndex - 1) * PageSize;
                int rowend = PageIndex * PageSize;

                if (rowbegin >= dt.Rows.Count){ return newdt; }

                if (rowend > dt.Rows.Count){ rowend = dt.Rows.Count; }

                for (int i = rowbegin; i <rowend; i++)
                {
                    DataRow newdr = newdt.NewRow();
                    DataRow dr = dt.Rows[i];
                    foreach (DataColumn column in dt.Columns)
                    {
                        newdr[column.ColumnName] = dr[column.ColumnName];
                    }
                    newdt.Rows.Add(newdr);
                }
            
            }
            catch (Exception ex)
            {
                Common.Units.Error.WriteErrorXml(dt + "分页出错:" + ex.Message);
            }
            return newdt;
        }
    }
}

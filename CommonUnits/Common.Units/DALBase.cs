using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Common.Units
{
    public class DALBase
    {
        public static int Delete(string table, int idValue, bool logicDel, string optUser)
        {
            if (logicDel)
            {
                return SQLServer.ExecuteNonQuery("update " + table + " set nvcOptState='delete',dtmOperation=@dtmOperation,nvcOptLoginCode=@nvcOptLoginCode where ID=@ID", new SqlParameter[] { SQLServer.CreateInputParam("@ID", idValue), SQLServer.CreateInputParam("@nvcOptLoginCode", optUser), SQLServer.CreateInputParam("@dtmOperation", DateTime.Now) });
            }
            
            return SQLServer.ExecuteNonQuery("delete from " + table + " where ID=@ID", new SqlParameter[] { SQLServer.CreateInputParam("@ID", idValue) });
        }

        protected static int Delete(string table, string field, string value, string Belong, string value1)
        {
            string cmdText = string.Format("delete from {0} where {1}='{2}' and {3}='{4}'", table, field, value, Belong, value1);
            try
            {
                return SQLServer.ExecuteNonQuery(cmdText, new SqlParameter[0]);
                //return 0;
            }
            catch
            {
                return 0;
            }
        }

        protected static int Delete(ref SqlTransaction trans, string table, string field, string value, string Belong, string value1)
        {
            int num2 = 0;
            string cmdText = string.Format("delete from {0} where {1}='{2}' and {3}='{4}'", table, field, value, Belong, value1);
            try
            {
                if (trans == null)
                {
                    SqlConnection connection = SQLServer.GetConnection();
                    connection.Open();
                    trans = connection.BeginTransaction();
                }
                SqlCommand cmd = new SqlCommand(cmdText)
                {
                    Connection = trans.Connection,
                    Transaction = trans
                };
                //SetParams(cmd, cmdParams);
                int num = SQLServer.ExecuteNonQuery(cmdText, new SqlParameter[0]);
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num2;
        }
        protected static int Delete(string table, string field, string value)
        {
            string cmdText = string.Format("delete from {0} where {1}='{2}'", table, field, value);
            try
            {
                return SQLServer.ExecuteNonQuery(cmdText, new SqlParameter[0]);
            }
            catch
            {
                return 0;
            }
        }
        protected static int DeleteByArray(string table, string field, string value)
        {
            string cmdText = string.Format("delete from {0} where {1} in ({2})", table, field, value);
            try
            {
                return SQLServer.ExecuteNonQuery(cmdText, new SqlParameter[0]);
            }
            catch
            {
                return 0;
            }
        }

        protected static string GetQueryText(string table, SortInfo sort, string filter)
        {
            try
            {
                return string.Format("select  * from {0} where {1} order by {2} {3} ", new object[] { table, filter, sort.field, sort.direction.ToString() });
            }
            catch
            {
                return "";
            }
        }

        protected static string GetQueryText(string table, long start, long limit, SortInfo sort)
        {
            try
            {
                if (start == 0)
                {
                    start = 1;
                }
                else
                {
                    start = start / limit + 1;
                }
                string s = string.Format("SELECT TOP {2} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {3} {4}) AS RowNumber,* FROM {0}) A WHERE RowNumber > {2}*({1}-1) ORDER BY {3} {4}", new object[] { table, start, limit, sort.field, sort.direction.ToString() });
                //string s = string.Format("select top {2} * from {0} where ID not in (select top {1} ID from {0} order by {3} {4}) order by {3} {4}", new object[] { table, start, limit, sort.field, sort.direction.ToString() });
                return s;
            }
            catch
            {
                return "";
            }
        }

        protected static string GetQueryText(string table, long start, long limit, SortInfo sort, string filter)
        {
            try
            {
                if (start == 0)
                {
                    start = 1;
                }
                else
                {
                    start = start / limit + 1;
                }
                string s = string.Format("SELECT TOP {2} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {4} {5}) AS RowNumber,* FROM {0} WHERE {3}) A WHERE RowNumber > {2}*({1}-1) ORDER BY {4} {5}", new object[] { table, start, limit, filter, sort.field, sort.direction.ToString() });
                return s;
                //return string.Format("select top {2} * from {0} where {3} and {4} not in (select top {1} {4} from {0} where {3} order by {4} {5}) order by {4} {5}", new object[] { table, start, limit, filter, sort.field, sort.direction.ToString() });
            }
            catch
            {
                return "";
            }
        }

        //lufn 20150818 监控专用

        protected static string GetQueryTextRM(string table, long start, long limit, SortInfo sort)
        {
            try
            {
                if (start == 0)
                {
                    start = 1;
                }
                else
                {
                    start = start / limit + 1;
                }
                string s = string.Format("SELECT TOP {2} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {3} {4}) AS RowNumber,* FROM {0} ) A WHERE RowNumber > {2}*({1}-1) ORDER BY {3} {4}", new object[] { table, start, limit, sort.field, sort.direction.ToString() });
                return s;
            }
            catch
            {
                return "";
            }
        }

        protected static string GetQueryTextRM(string table, long start, long limit, SortInfo sort, string filter)
        {
            try
            {
                if (start == 0)
                {
                    start = 1;
                }
                else
                {
                    start = start / limit + 1;
                }
                string s = string.Format("SELECT TOP {2} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {4} {5}) AS RowNumber,* FROM {0}  WHERE {3}) A WHERE RowNumber > {2}*({1}-1) ORDER BY {4} {5}", new object[] { table, start, limit, filter, sort.field, sort.direction.ToString() });
                return s;
            }
            catch
            {
                return "";
            }
        }

        public static long Total(string table)
        {
            string cmdText = string.Format("select Count(*) from {0}", table);
            try
            {
                return Convert.ToInt64(SQLServer.ExecuteScalar(cmdText, new SqlParameter[0]));
            }
            catch
            {
                return 0L;
            }
        }

        public static long TotalRM(string table, string filter)
        {
            string cmdText = string.Format("select Count(*) from {0} where {1}", table, filter);
            try
            {
                return Convert.ToInt64(SQLServer.ExecuteScalar(cmdText, new SqlParameter[0]));
            }
            catch
            {
                return 0L;
            }
        }



        public static string GetQueryText(string tableName, SortInfo sort, int start, int limit, string gField, string gValue)
        {
            if (limit > 0)
            {
                if (start == 0)
                {
                    start = 1;
                }
                else
                {
                    start = start / limit + 1;
                }
                return string.Format("SELECT TOP {2} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {5} {6}) AS RowNumber,* FROM {0} WHERE {3}='{4}') A WHERE RowNumber > {2}*({1}-1) ORDER BY {5} {6}", new object[] { tableName, start, limit, gField, gValue, sort.field, sort.direction.ToString() });
                //return string.Format("select top {6} * from {0} where {3}='{4}' and {1} not in (select top {5} {1} from {0} where {3}='{4}' order by {1} {2}) order by {1} {2}", new object[] { tableName, sort.field, sort.direction.ToString(), gField, gValue, start, limit });
            }
            return string.Format("select * from {0} where {3}='{4}' order by {1} {2}", new object[] { tableName, sort.field, sort.direction.ToString(), gField, gValue });
        }

        public static int intTotal(string tableName)
        {
            string cmdText = string.Format("select Count(*) from {0}", tableName);
            int num = 0;
            try
            {
                SqlCommand command = new SqlCommand(cmdText);
                SqlConnection connection = SQLServer.GetConnection();
                connection.Open();
                command.Connection = connection;
                object obj2 = command.ExecuteScalar();
                connection.Close();
                num = Convert.ToInt32(obj2);
            }
            catch
            {
            }
            return num;
        }

        public static int intTotal(string tableName, string gField, string gValue)
        {
            string cmdText = string.Format("select Count(*) from {0} where {1}='{2}'", tableName, gField, gValue);
            int num = 0;
            try
            {
                SqlCommand command = new SqlCommand(cmdText);
                SqlConnection connection = SQLServer.GetConnection();
                connection.Open();
                command.Connection = connection;
                object obj2 = command.ExecuteScalar();
                connection.Close();
                num = Convert.ToInt32(obj2);
            }
            catch
            {
            }
            return num;
        }

        public static long Total(string table, string filter)
        {
            string cmdText = string.Format("select Count(*) from {0} where {1}", table, filter);
            try
            {
                return Convert.ToInt64(SQLServer.ExecuteScalar(cmdText, new SqlParameter[0]));
            }
            catch
            {
                return 0L;
            }
        }

        #region 重写的方法
        public static string GetCmdText(string table, long start, long limit, SortInfo sort)
        {
            if (start == 0)
            {
                start = 1;
            }
            try
            {
                return string.Format("select *from (select ROW_NUMBER() over(order by {3}) as paixu,* from {0})D WHERE paixu between ({1}-1)*{2}+1 and {1}*{2} order by {3} {4}", new object[] { table, start, limit, sort.field, sort.direction.ToString() });
            }
            catch
            {
                return "";
            }
        }

        public static string GetCmdText(string table, long start, long limit, SortInfo sort, string filter)
        {
            if (start == 0)
            {
                start = 1;
            }
            try
            {
                return string.Format("select *from (select ROW_NUMBER() over(order by {3}) as paixu,* from {0} where {5})D WHERE paixu between ({1}-1)*{2}+1 and {1}*{2} order by {3} {4}", new object[] { table, start, limit, sort.field, sort.direction.ToString(), filter });
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}

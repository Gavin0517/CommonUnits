using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Common.Units
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
   public class SQLServer
    {
        public static SqlParameter CreateInputParam(string paramName, object value)
        {
            return CreateParam(ParameterDirection.Input, paramName, value);
        }

        private static SqlParameter CreateParam(ParameterDirection dir, string paramName, object value)
        {
            return new SqlParameter(paramName, value) { Direction = dir };
        }

        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParams)
        {
            int num2;
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                SetParams(cmd, cmdParams);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return num2;
        }

        public static int ExecuteNonQuery(ref SqlTransaction trans, string cmdText, params SqlParameter[] cmdParams)
        {
            int num2;
            try
            {
                if (trans == null)
                {
                    SqlConnection connection = GetConnection();
                    connection.Open();
                    trans = connection.BeginTransaction();
                }
                SqlCommand cmd = new SqlCommand(cmdText)
                {
                    Connection = trans.Connection,
                    Transaction = trans
                };
                SetParams(cmd, cmdParams);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num2;
        }

        public static int ExecuteNonQuery(SqlTransaction trans, string cmdText, params SqlParameter[] cmdParams)
        {
            int num2;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText)
                {
                    Connection = trans.Connection,
                    Transaction = trans
                };
                SetParams(cmd, cmdParams);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num2;
        }

        public static object ExecuteObject(string table, string field, string filter, string value, params KeyValuePair<string, string>[] filters)
        {
            using (SqlConnection connection = GetConnection())
            {
                object obj2 = null;
                try
                {
                    connection.Open();
                    string cmdText = string.Format("select {1} from {0} where {2}='{3}'", new object[] { table, field, filter, value });
                    foreach (KeyValuePair<string, string> pair in filters)
                    {
                        cmdText = cmdText + string.Format(" and {0}='{1}'", pair.Key, pair.Value);
                    }
                    obj2 = ExecuteScalar(cmdText, new SqlParameter[0]);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
                return obj2;
            }
        }

        public static DataRow ExecuteRow(string cmdText, params SqlParameter[] cmdParams)
        {
            using (SqlConnection connection = GetConnection())
            {
                DataRow row = null;
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(cmdText, connection);
                    if (cmdParams.Length > 0)
                    {
                        SetParams(cmd, cmdParams);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet.Tables[0].Rows.Count != 1)
                        {
                            throw new Exception("所查询的内容包含多条记录.");
                        }
                        row = dataSet.Tables[0].Rows[0];
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
                return row;
            }
        }

        public static DataRow ExecuteRow(string table, string filter, string value, params KeyValuePair<string, string>[] filters)
        {
            using (SqlConnection connection = GetConnection())
            {
                DataRow row = null;
                try
                {
                    connection.Open();
                    string cmdText = string.Format("select * from {0} where {1}='{2}'", table, filter, value);
                    foreach (KeyValuePair<string, string> pair in filters)
                    {
                        cmdText = cmdText + string.Format(" and {0}='{1}'", pair.Key, pair.Value);
                    }
                    SqlCommand selectCommand = new SqlCommand(cmdText, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count == 1)
                    {
                        if (dataSet.Tables[0].Rows.Count != 1)
                        {
                            throw new Exception("所查询的内容包含多条记录.");
                        }
                        row = dataSet.Tables[0].Rows[0];
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
                return row;
            }
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParams)
        {
            object obj3;
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                if (cmdParams.Length > 0)
                {
                    SetParams(cmd, cmdParams);
                }
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                obj3 = obj2;
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return obj3;
        }

        public static DataTable ExecuteTable(string cmdText)
        {
            DataTable table;
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(cmd).Fill(dataSet);
                if (dataSet.Tables.Count > 0)
                {
                    return dataSet.Tables[0];
                }
                table = null;
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return table;
        }

        public static DataTable ExecuteTable(string cmdText, params SqlParameter[] cmdParams)
        {
            DataTable table;
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                if (cmdParams.Length > 0)
                {
                    SetParams(cmd, cmdParams);
                }
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(cmd).Fill(dataSet);
                if (dataSet.Tables.Count > 0)
                {
                    return dataSet.Tables[0];
                }
                table = null;
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return table;
        }

        public static DataTable ExecuteTable(string cmdText,bool storeFlag, params SqlParameter[] cmdParams)
        {
            if (!storeFlag)
                ExecuteTable(cmdText,cmdParams);
            DataTable table;
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmdParams.Length > 0)
                {
                    SetParams(cmd, cmdParams);
                }
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(cmd).Fill(dataSet);
                if (dataSet.Tables.Count > 0)
                {
                    return dataSet.Tables[0];
                }
                table = null;
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return table;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServerHelper"].ConnectionString);
        }
  
        private static void SetParams(SqlCommand cmd, SqlParameter[] cmdParams)
        {
            if (cmdParams != null)
            {
                foreach (SqlParameter parameter in cmdParams)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        ///<summary>
        ///执行添删改(存储过程)
        ///xc.ma
        ///lufn(2014-05-25)
        ///</summary>
        ///<param name="proname"></param>
        ///<param name="param"></param>
        ///<returns></returns>
        public static bool ExecuteProcADU(string proname, params SqlParameter[] param)
        {
            SqlConnection myconn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServerHelper"].ConnectionString);
            myconn.Open();
            SqlCommand mycomm = myconn.CreateCommand();
            foreach (SqlParameter mypara in param)
            {
                mycomm.Parameters.Add(mypara);
            }
            mycomm.CommandType = CommandType.StoredProcedure;
            mycomm.CommandText = proname;
            try
            {
                mycomm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
                return false;
            }
            finally
            {
                myconn.Close();
                mycomm.Dispose();
                mycomm.Parameters.Clear();
            }
        }
        /// <summary>
        /// 执行存储过程返回一个DataTable
        /// 2011-11-19
        /// </summary>
        /// <param name="proname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable ExecuteSelect(string procname, SqlParameter[] param)
        {
            DataSet myds = new DataSet();
            SqlDataAdapter mysda;
            SqlConnection myconn = GetConnection();
            myconn.Open();
            SqlCommand mycomm = myconn.CreateCommand();
            foreach (SqlParameter mypara in param)
            {
                mycomm.Parameters.Add(mypara);
            }
            mycomm.CommandType = CommandType.StoredProcedure;
            mycomm.CommandText = procname;

            try
            {
                mysda = new SqlDataAdapter(procname, myconn);
                mysda.SelectCommand = mycomm;
                mysda.Fill(myds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                myconn.Close();
                mycomm.Dispose();
            }
            return myds.Tables[0];
        }

        /// <summary>
        /// SqlGetDataTable [ 执行查询-返DataTable]
        /// </summary>
        /// <param name="proc">存储过程名称</param>
        /// <param name="type">存储过程类型</param>
        /// <param name="paramValue">参数值-[字符串数组]{详细参数请看P_AspNetPage存储过程里面的注释}</param>
        /// <param name="OutTotalCount">out总记录数</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteTable(string proc, CommandType type, string[] paramValue, out int OutTotalCount)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection connection = GetConnection();
                SqlCommand cmd = new SqlCommand(proc, connection);
                //分页开始 
                SqlParameter[] myParms = new SqlParameter[10];

                myParms[0] = new SqlParameter("@TableName", SqlDbType.VarChar, 50);
                myParms[0].Value = paramValue[0];

                myParms[1] = new SqlParameter("@FieldList", SqlDbType.VarChar, 50);
                myParms[1].Value = paramValue[1];

                myParms[2] = new SqlParameter("@PrimaryKey", SqlDbType.VarChar, 50);
                myParms[2].Value = paramValue[2];

                myParms[3] = new SqlParameter("@Where", SqlDbType.VarChar, 500);
                myParms[3].Value = paramValue[3];

                myParms[4] = new SqlParameter("@Order", SqlDbType.VarChar, 50);
                myParms[4].Value = paramValue[4];

                myParms[5] = new SqlParameter("@SortType", SqlDbType.Int, 4);
                myParms[5].Value = paramValue[5];

                myParms[6] = new SqlParameter("@RecorderCount", SqlDbType.Int, 4);
                myParms[6].Value = paramValue[6];

                myParms[7] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
                myParms[7].Value = paramValue[7];

                myParms[8] = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
                myParms[8].Value = paramValue[8];

                myParms[9] = new SqlParameter("@TotalCount", SqlDbType.Int, 4);
                myParms[9].Direction = ParameterDirection.Output;
                foreach (SqlParameter parameter in myParms)
                {
                    cmd.Parameters.Add(parameter);
                }
                cmd.CommandType = type;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //out 总记录数
                OutTotalCount = Convert.ToInt32(myParms[9].Value);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }


      
    }
}

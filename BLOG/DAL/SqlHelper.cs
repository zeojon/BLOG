using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    class SqlHelper
    {
        //从Web.config中读取数据库连接字符春
        private String ConnStr = "server=.;database=blog;uid=sa;pwd=12345";

        private SqlConnection conn = null;

        /// <summary>
        /// 将查询结果集填充到DataTable
        /// </summary>
        /// <param name="query">查询T-Sql</param>
        /// <returns></returns>
        public DataTable FillDataTable(String query)
        {
            DataTable dt = new DataTable();
            using (conn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                SqlDataAdapter ada = new SqlDataAdapter();
                ada.SelectCommand = cmd;
                ada.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 将查询结果集填充到DataSet
        /// </summary>
        /// <param name="query">查询T-Sql,可以是多个Select语句</param>
        /// <returns></returns>
        public DataSet FillDataSet(String query)
        {
            DataSet ds = new DataSet();
            using (conn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                SqlDataAdapter ada = new SqlDataAdapter();
                ada.SelectCommand = cmd;
                ada.Fill(ds);
            }
            return ds;
        }

        /// <summary>
        /// 执行insert、update、delete、truncate语句
        /// </summary>
        /// <param name="commandText">insert、update、delete、truncate语句</param>
        public void ExecuteNonQuery(String commandText)
        {
            using (conn = new SqlConnection(ConnStr))
            {
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = commandText;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                finally
                {
                    tran.Dispose();
                }
            }
        }

    }
}

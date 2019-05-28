using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Scan_01
{
    public static partial class SqlHelper
    {

        public readonly static string connStr = ConfigurationManager.ConnectionStrings["dipian_data"].ConnectionString;

        public static int ExecuteNoneQuery(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(ps);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        #region 返回结果集中第一行的第一列+ExecuteScalar(string sql, params SqlParameter[] ps)
        public static object ExecuteScalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(ps);

                conn.Open();
                return cmd.ExecuteScalar();

            }
        }
        #endregion

        public static DataTable ExecuteTable(string sql, params SqlParameter[] ps)//返回数据集
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddRange(ps);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        //public static SqlDataReader ExecuteReader(string sql, )
        //{
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        //创建命令对象
        //        SqlCommand comm = new SqlCommand(sql, conn);
        //        //创建一个读取器对象，这个对象可以从服务器中每一次读取出一行数据
        //        SqlDataReader reader = comm.ExecuteReader();
        //        //数据还需要去循环读取
        //    }
        //}
    }
}

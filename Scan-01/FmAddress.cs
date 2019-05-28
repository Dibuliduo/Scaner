using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Scan_01
{
    public partial class FmAddress : Form
    {
        public FmAddress()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //string sql = "select * from dipian_address";
            //DataTable dt = SqlHelper.ExecuteTable(sql);
            ////this.dataGridView1.AutoGenerateColumns = false;//防止乱序,将这句代码写在下一句代码之前
            //dataGridView1.DataSource = dt;
            ////txtSum.Text = dataGridView1.Rows.Count.ToString();//返回行数
        }

        private void FmAddress_Load(object sender, EventArgs e)
        {
            string sql = "select * from dipian_address";
            DataTable dt = SqlHelper.ExecuteTable(sql);
            this.dataGridView1.AutoGenerateColumns = false;//防止乱序,将这句代码写在下一句代码之前
            dataGridView1.DataSource = dt;
            //txtSum.Text = dataGridView1.Rows.Count.ToString();//返回行数
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //    //判断dipian_address表里面有没有相同行
            //    for (int row = 0; row < 59; row++)
            //    {
            //        string rowNum = "select * from dipian_address where dpAddressX = @dpAddressX";
            //        SqlParameter[] paraRow =
            //        {
            //                new SqlParameter("@dpAddressX", row),
            //         };
            //        int dtAddressRow = SqlHelper.ExecuteTable(rowNum, paraRow).Rows.Count;

            //        if (dtAddressRow == 0)
            //        {
            //            for (int column = 0; column < 11; column++)
            //            {
            //                string sqlAddressNum = "select * from dipian_address where dpAddressY = @dpAddressY";
            //                SqlParameter[] paraColumn =
            //                {
            //                    new SqlParameter("@dpAddressY", column),
            //                };
            //                int dtAddressColumn = SqlHelper.ExecuteTable(sqlAddressNum, paraColumn).Rows.Count;
            //                if (dtAddressColumn == 0)
            //                {
            //                    string sql = "insert into dipian_address(dpAddressX, dpAddressY, dpAddressStatus) values(@dpAddressX,@dpAddressY,@dpAddressStatus)";
            //                    SqlParameter[] ps =
            //                    {
            //                        new SqlParameter("@dpAddressX", row),
            //                        new SqlParameter("@dpAddressY", column),
            //                        new SqlParameter("@dpAddressStatus", txtStatus.Text.Trim()),
            //                    };

            //                    if (SqlHelper.ExecuteNoneQuery(sql, ps) == 1)
            //                    {
            //                        sql = "select * from dipian_data";//刷新datagridview
            //                        DataTable dt = SqlHelper.ExecuteTable(sql);//集合并生成表
            //                        dataGridView1.DataSource = dt;//把生成表放入dgv
            //                        txtSum.Text = dataGridView1.Rows.Count.ToString();
            //                    }
            //                }
            //            }
            //        }
            //    }


            ////判断dipian_address表里面有没有相同行
            //string rowNum = "select * from dipian_address where dpAddressNum = @dpAddressNum";
            //SqlParameter[] paraRow =
            //{
            //            new SqlParameter("@dpAddressNum", ),
            //};
            //int dtAddressRow = SqlHelper.ExecuteTable(rowNum, paraRow).Rows.Count;

         }
     }
 }

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
    public partial class FormAdd : Form
    {
        public FormAdd()
        {
            InitializeComponent();
        }

        private void FromAdd_Load(object sender, EventArgs e)
        {
           //string sql = "select * from data"; 
        }


        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string sql = "insert into dipian_data(dpVersion,dpliaohao,dpLayer,dpX,dpY,dpNum,dpType,dpTime) values(@Version,@liaohao,@Layer,@X,@Y,@Num,@Type,@Time)";
            SqlParameter[] ps = 
            {
                new SqlParameter("@Version", txtdpVersion.Text.Trim()),
                new SqlParameter("@liaohao", txtdpliaohao.Text.Trim()),
                new SqlParameter("@Layer", txtdpLayer.Text.Trim()),
                new SqlParameter("@X", txtdpX.Text.Trim()),
                new SqlParameter("@Y", txtdpY.Text.Trim()),
                new SqlParameter("@Num", txtNum.Text.Trim()),
                new SqlParameter("@Type", txtType.Text.Trim()),
                new SqlParameter("@Time", txtTime.Text.Trim()),
            };
                int  result = SqlHelper.ExecuteNoneQuery(sql,ps);
                if (result > 0)
                {
                    //from
                    this.Close();
                }
                else
                {
                    MessageBox.Show("添加失败"); 
                }

            
        }
    }
}

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
    public partial class FrmLog : Form
    {
        public FrmLog()
        {
            InitializeComponent();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        public static bool Login = false;

        public void Button1_Click(object sender, EventArgs e)
        {
            string sqlLog = "select count(*) from dipian_Login where dpLogId = @dpLogId";
            SqlParameter[] ps =
            {
                new SqlParameter("@dpLogId", txtLogUserName.Text.Trim()),
            };


            Login = !SqlHelper.ExecuteNoneQuery(sqlLog, ps).Equals(0);

            if (Login)
            {
                MessageBox.Show("登陆成功");
            }
            else
            {
                MessageBox.Show("请扫描二维码或者输入工号");
            }
        }

        private void FrmLog_Load(object sender, EventArgs e)
        {

        }
    }
}

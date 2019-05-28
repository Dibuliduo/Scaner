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
    public partial class FrmFilter : Form
    {
        public FrmFilter()
        {
            InitializeComponent();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            string sql = "select * from dipian_data";

            if (txtSizing_X1.Text.Trim() != "")//第一个txtX1不为空
            {
                if (txtSizing_X2.Text.Trim() != "")//第二个txtX2不为空
                {
                    sql = sql + " where (dpx>@dpX1 and dpX <@dpX2 ) ";

                    if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " and (dpY>@dpY1 and dpY <@dpY2 ) ";
                        }
                        else//第二个txtY2为空
                        {
                            sql = sql + " and (dpY>@dpY1 ) ";
                        }
                    }
                    else//第一个txtY1为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " and ( dpY <@dpY2 ) ";
                        }
                    }
                }
                else//第二个txtX2为空
                {
                    sql = sql + " where (dpx>@dpX1) ";

                    if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " and (dpY>@dpY1 and dpY <@dpY2 ) ";
                        }
                        else//第二个txtY2为空
                        {
                            sql = sql + " and (dpY>@dpY1 ) ";
                        }
                    }
                    else
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " and ( dpY <@dpY2 ) ";
                        }
                    }
                }
            }
            else//第一个txtX1为空
            {
                if (txtSizing_X2.Text.Trim() != "")//第二个txtX2不为空
                {
                    sql = sql + " where ( dpX <@dpX2 ) ";

                    if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " and (dpY>@dpY1 and dpY <@dpY2 ) ";
                        }
                        else//第二个txtY2为空
                        {
                            sql = sql + " and (dpY>@dpY1 ) ";
                        }
                    }
                    else//第一个txtY1为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " and ( dpY <@dpY2 ) ";
                        }
                    }
                }
                else//第二个txtX2为空
                {
                    if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " where (dpY>@dpY1 and dpY <@dpY2 ) ";
                        }
                        else//第二个txtY2为空
                        {
                            sql = sql + " where (dpY>@dpY1 ) ";
                        }
                    }
                    else//第一个txtY1为空
                    {
                        if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
                        {
                            sql = sql + " where ( dpY <@dpY2 ) ";
                        }
                    }
                }
            }

            SqlParameter[] ps =
            {
                new SqlParameter("@dpX1", txtSizing_X1.Text.Trim()),
                new SqlParameter("@dpX2", txtSizing_X2.Text.Trim()),
                new SqlParameter("@dpY1", txtSizing_Y1.Text.Trim()),
                new SqlParameter("@dpY2", txtSizing_Y2.Text.Trim()),
            };

            DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            this.dataGridView1.DataSource = dt;


         }

         private void FrmFilter_Load(object sender, EventArgs e)
         {
            //加载窗体触发的功能:
            //1.窗口位置形状参数
            #region 窗口位置
            this.CenterToScreen();
            #endregion

            //2.datagridview功能区加载
            #region 初始化datagridview + string sql = "select * from dipian_data";
            //初始化datagridview
            string sql = "select * from dipian_data";
            DataTable dt = SqlHelper.ExecuteTable(sql);
            this.dataGridView1.AutoGenerateColumns = false;//防止乱序,将这句代码写在下一句代码之前
            dataGridView1.DataSource = dt;
            txtSum.Text = dataGridView1.Rows.Count.ToString();//返回表格中一共有多少物料
            #endregion
            #region 把总数量,存入数量,取出数量实时返回
            string sqlSum = "select * from dipian_data";//刷新datagridview
            DataTable dtSum = SqlHelper.ExecuteTable(sqlSum);//集合并生成表
            dataGridView1.DataSource = dtSum;//把生成表放入dgv

            string sqlStorSum = "select * from dipian_data where dpStatus = '存入'";
            string sqlTakeOutSum = "select * from dipian_data where dpStatus = '取出'";

            txtSaveSum.Text = SqlHelper.ExecuteTable(sqlStorSum).Rows.Count.ToString();
            txtTakeOutSum.Text = SqlHelper.ExecuteTable(sqlTakeOutSum).Rows.Count.ToString();
            txtSum.Text = dataGridView1.Rows.Count.ToString();
            #endregion

        }

        private void TextBox10_TextChanged(object sender, EventArgs e)
        {
            //#region 字符分割
            ////字符分割并传入textbox
            //string canshu = txtBarcodeFilter.Text.Trim();
            //string StrUpper = canshu.ToUpper();//转成大写
            //string[] Str = new string[StrUpper.Length];//创建数组
            //if (txtBarcodeFilter.TextLength == 0 || txtBarcodeFilter.TextLength < 26)
            //{
            //    //MessageBox.Show("请扫描二维码!");
            //    if (txtBarcodeFilter.TextLength != 0)
            //    {
            //        //分割条形码
            //        Str[0] = StrUpper.Substring(0, 4);
            //        Str[1] = StrUpper.Substring(4, 4);
            //        Str[2] = StrUpper.Substring(9, 7);
            //        Str[3] = StrUpper.Substring(17, 7);
            //        //Str[4] = StrUpper.Substring(16, 1);
            //        //Str[5] = StrUpper.Substring(17, 5);
            //        //Str[6] = StrUpper.Substring(22, 5);
            //        //Str[7] = StrUpper.Substring(27, 4);

            //        //把截取的字符串赋值给textbox
            //        txtFilterliaohao.Text = Str[2].ToString();
            //        //txtdpVersion1.Text = Str[1].ToString();
            //        txtFilterLayer.Text = Str[1].ToString();
            //        //txtdpX1.Text = Str[3].ToString();
            //        //txtdpY1.Text = Str[4].ToString();
            //        //txtNum1.Text = Str[5].ToString();
            //        //txtType1.Text = Str[6].ToString();
            //        //txtTime1.Text = Str[7].ToString();

            //        #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //        string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //        SqlParameter[] ps =
            //        {
            //            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //        };

            //        DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //        this.dataGridView1.DataSource = dt;
            //        #endregion
            //    }
            //    else
            //    {
            //        MessageBox.Show("请扫描条形码");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("请扫描条形码");
            //}
            //#endregion
        }

        private void FrmFilter_Activated(object sender, EventArgs e)
        {
            txtFilterBarcode.Focus();
        }

        private void TxtFilterBarcode_TextChanged(object sender, EventArgs e)
        {
            //#region 字符分割
            ////字符分割并传入textbox
            //string canshu = txtFilterBarcode.Text.Trim();
            //string StrUpper = canshu.ToUpper();//转成大写
            //string[] Str = new string[StrUpper.Length];//创建数组
            //if (txtFilterBarcode.TextLength == 0 || txtFilterBarcode.TextLength < 26)
            //{
            //    //MessageBox.Show("请扫描二维码!");
            //    if (txtFilterBarcode.TextLength != 0)
            //    {
            //        //分割条形码
            //        Str[0] = StrUpper.Substring(0, 4);
            //        Str[1] = StrUpper.Substring(4, 4);
            //        Str[2] = StrUpper.Substring(9, 7);
            //        Str[3] = StrUpper.Substring(17, 7);
            //        //Str[4] = StrUpper.Substring(16, 1);
            //        //Str[5] = StrUpper.Substring(17, 5);
            //        //Str[6] = StrUpper.Substring(22, 5);
            //        //Str[7] = StrUpper.Substring(27, 4);

            //        //把截取的字符串赋值给textbox
            //        txtFilterliaohao.Text = Str[2].ToString();
            //        //txtdpVersion1.Text = Str[1].ToString();
            //        txtFilterLayer.Text = Str[1].ToString();
            //        //txtdpX1.Text = Str[3].ToString();
            //        //txtdpY1.Text = Str[4].ToString();
            //        //txtNum1.Text = Str[5].ToString();
            //        //txtType1.Text = Str[6].ToString();
            //        //txtTime1.Text = Str[7].ToString();

            //        #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //        string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //        SqlParameter[] ps =
            //        {
            //            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //        };

            //        DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //        this.dataGridView1.DataSource = dt;
            //        #endregion
            //    }
            //    else
            //    {
            //        MessageBox.Show("请扫描条形码");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("请扫描条形码");
            //}
            //#endregion
        }

        private void TxtFilterBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            //#region 字符分割
            ////字符分割并传入textbox
            //string canshu = txtFilterBarcode.Text.Trim();
            //string StrUpper = canshu.ToUpper();//转成大写
            //string[] Str = new string[StrUpper.Length];//创建数组
            //if (txtFilterBarcode.TextLength == 0 || txtFilterBarcode.TextLength < 26)
            //{
            //    //MessageBox.Show("请扫描二维码!");
            //    if (txtFilterBarcode.TextLength != 0)
            //    {
            //        //分割条形码
            //        Str[0] = StrUpper.Substring(0, 4);
            //        Str[1] = StrUpper.Substring(4, 4);
            //        Str[2] = StrUpper.Substring(9, 7);
            //        Str[3] = StrUpper.Substring(17, 7);
            //        //Str[4] = StrUpper.Substring(16, 1);
            //        //Str[5] = StrUpper.Substring(17, 5);
            //        //Str[6] = StrUpper.Substring(22, 5);
            //        //Str[7] = StrUpper.Substring(27, 4);

            //        //把截取的字符串赋值给textbox
            //        txtFilterliaohao.Text = Str[2].ToString();
            //        //txtdpVersion1.Text = Str[1].ToString();
            //        txtFilterLayer.Text = Str[1].ToString();
            //        //txtdpX1.Text = Str[3].ToString();
            //        //txtdpY1.Text = Str[4].ToString();
            //        //txtNum1.Text = Str[5].ToString();
            //        //txtType1.Text = Str[6].ToString();
            //        //txtTime1.Text = Str[7].ToString();

            //        #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //        string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //        SqlParameter[] ps =
            //        {
            //            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //        };

            //        DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //        this.dataGridView1.DataSource = dt;
            //        #endregion
            //    }
            //    else
            //    {
            //        MessageBox.Show("请扫描条形码");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("请扫描条形码");
            //}
            //#endregion
        }

        private void TxtFilterBarcode_MouseLeave(object sender, EventArgs e)
        {
            //#region 字符分割
            ////字符分割并传入textbox
            //string canshu = txtFilterBarcode.Text.Trim();
            //string StrUpper = canshu.ToUpper();//转成大写
            //string[] Str = new string[StrUpper.Length];//创建数组
            //if (txtFilterBarcode.TextLength == 0 || txtFilterBarcode.TextLength < 26)
            //{
            //    //MessageBox.Show("请扫描二维码!");
            //    if (txtFilterBarcode.TextLength != 0)
            //    {
            //        //分割条形码
            //        Str[0] = StrUpper.Substring(0, 4);
            //        Str[1] = StrUpper.Substring(4, 4);
            //        Str[2] = StrUpper.Substring(9, 7);
            //        Str[3] = StrUpper.Substring(17, 7);
            //        //Str[4] = StrUpper.Substring(16, 1);
            //        //Str[5] = StrUpper.Substring(17, 5);
            //        //Str[6] = StrUpper.Substring(22, 5);
            //        //Str[7] = StrUpper.Substring(27, 4);

            //        //把截取的字符串赋值给textbox
            //        txtFilterliaohao.Text = Str[2].ToString();
            //        //txtdpVersion1.Text = Str[1].ToString();
            //        txtFilterLayer.Text = Str[1].ToString();
            //        //txtdpX1.Text = Str[3].ToString();
            //        //txtdpY1.Text = Str[4].ToString();
            //        //txtNum1.Text = Str[5].ToString();
            //        //txtType1.Text = Str[6].ToString();
            //        //txtTime1.Text = Str[7].ToString();

            //        #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //        string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //        SqlParameter[] ps =
            //        {
            //            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //        };

            //        DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //        this.dataGridView1.DataSource = dt;
            //        #endregion
            //    }
            //    else
            //    {
            //        MessageBox.Show("请扫描条形码");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("请扫描条形码");
            //}
            //#endregion
        }

        private void TxtFilterBarcode_Leave(object sender, EventArgs e)
        {
            //#region 字符分割
            ////字符分割并传入textbox
            //string canshu = txtFilterBarcode.Text.Trim();
            //string StrUpper = canshu.ToUpper();//转成大写
            //string[] Str = new string[StrUpper.Length];//创建数组
            //if (txtFilterBarcode.TextLength > 0 && txtFilterBarcode.TextLength < 26)
            //{
            //    //分割条形码
            //    Str[0] = StrUpper.Substring(0, 4);
            //    Str[1] = StrUpper.Substring(4, 4);
            //    Str[2] = StrUpper.Substring(9, 7);
            //    Str[3] = StrUpper.Substring(17, 7);
            //    //Str[4] = StrUpper.Substring(16, 1);
            //    //Str[5] = StrUpper.Substring(17, 5);
            //    //Str[6] = StrUpper.Substring(22, 5);
            //    //Str[7] = StrUpper.Substring(27, 4);

            //    //把截取的字符串赋值给textbox
            //    txtFilterliaohao.Text = Str[2].ToString();
            //    //txtdpVersion1.Text = Str[1].ToString();
            //    txtFilterLayer.Text = Str[1].ToString();
            //    //txtdpX1.Text = Str[3].ToString();
            //    //txtdpY1.Text = Str[4].ToString();
            //    //txtNum1.Text = Str[5].ToString();
            //    //txtType1.Text = Str[6].ToString();
            //    //txtTime1.Text = Str[7].ToString();

            //    #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //    string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //    SqlParameter[] ps =
            //    {
            //            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //        };

            //    DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //    this.dataGridView1.DataSource = dt;

            //    txtSizing_X1.Focus();
            //    #endregion
            //}
            //else
            //{
            //    MessageBox.Show("请扫描条形码");
            //}
            //#endregion
        }

        private void TxtFilterBarcode_Validated(object sender, EventArgs e)
        {
            //if (!txtFilterBarcode.Text.Equals(""))
            //{
            //    #region 字符分割
            //    //字符分割并传入textbox
            //    string canshu = txtFilterBarcode.Text.Trim();
            //    string StrUpper = canshu.ToUpper();//转成大写
            //    string[] Str = new string[StrUpper.Length];//创建数组
            //    if (txtFilterBarcode.TextLength == 0 || txtFilterBarcode.TextLength < 26)
            //    {
            //        //MessageBox.Show("请扫描二维码!");
            //        if (txtFilterBarcode.TextLength != 0)
            //        {
            //            //分割条形码
            //            Str[0] = StrUpper.Substring(0, 4);
            //            Str[1] = StrUpper.Substring(4, 4);
            //            Str[2] = StrUpper.Substring(9, 7);
            //            Str[3] = StrUpper.Substring(17, 7);
            //            //Str[4] = StrUpper.Substring(16, 1);
            //            //Str[5] = StrUpper.Substring(17, 5);
            //            //Str[6] = StrUpper.Substring(22, 5);
            //            //Str[7] = StrUpper.Substring(27, 4);

            //            //把截取的字符串赋值给textbox
            //            txtFilterliaohao.Text = Str[2].ToString();
            //            //txtdpVersion1.Text = Str[1].ToString();
            //            txtFilterLayer.Text = Str[1].ToString();
            //            //txtdpX1.Text = Str[3].ToString();
            //            //txtdpY1.Text = Str[4].ToString();
            //            //txtNum1.Text = Str[5].ToString();
            //            //txtType1.Text = Str[6].ToString();
            //            //txtTime1.Text = Str[7].ToString();

            //            #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //            string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //            SqlParameter[] ps =
            //            {
            //            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //        };

            //            DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //            this.dataGridView1.DataSource = dt;

            //            txtSizing_X1.Focus();
            //            #endregion
            //        }
            //        else
            //        {
            //            MessageBox.Show("请扫描条形码");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("请扫描条形码");
            //    }
            //    #endregion
            //}
        }

        private void TxtFilterBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    #region 字符分割
            //    //字符分割并传入textbox
            //    string canshu = txtFilterBarcode.Text.Trim();
            //    string StrUpper = canshu.ToUpper();//转成大写
            //    string[] Str = new string[StrUpper.Length];//创建数组
            //    if (txtFilterBarcode.TextLength == 0 || txtFilterBarcode.TextLength < 26)
            //    {
            //        //MessageBox.Show("请扫描二维码!");
            //        if (txtFilterBarcode.TextLength != 0)
            //        {
            //            //分割条形码
            //            Str[0] = StrUpper.Substring(0, 4);
            //            Str[1] = StrUpper.Substring(4, 4);
            //            Str[2] = StrUpper.Substring(9, 7);
            //            Str[3] = StrUpper.Substring(17, 7);
            //            //Str[4] = StrUpper.Substring(16, 1);
            //            //Str[5] = StrUpper.Substring(17, 5);
            //            //Str[6] = StrUpper.Substring(22, 5);
            //            //Str[7] = StrUpper.Substring(27, 4);

            //            //把截取的字符串赋值给textbox
            //            txtFilterliaohao.Text = Str[2].ToString();
            //            //txtdpVersion1.Text = Str[1].ToString();
            //            txtFilterLayer.Text = Str[1].ToString();
            //            //txtdpX1.Text = Str[3].ToString();
            //            //txtdpY1.Text = Str[4].ToString();
            //            //txtNum1.Text = Str[5].ToString();
            //            //txtType1.Text = Str[6].ToString();
            //            //txtTime1.Text = Str[7].ToString();

            //            #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //            string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
            //            SqlParameter[] ps =
            //            {
            //                new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
            //                new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
            //            };

            //            DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
            //            this.dataGridView1.DataSource = dt;
            //            #endregion
            //        }
            //        else
            //        {
            //            MessageBox.Show("请扫描条形码");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("请扫描条形码");
            //    }
            //    #endregion
            //}
        }

        private void TxtFilterVersion_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void TxtFilterType_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void TxtFilterTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void TxtFilterStor_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {

        }

        private void TxtFilterTakeout_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label8_Click(object sender, EventArgs e)
        {

        }

        private void TxtFilterStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label9_Click(object sender, EventArgs e)
        {

        }

        private void TextBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtFilterBarcode_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                #region 字符分割
                //字符分割并传入textbox
                string canshu = txtFilterBarcode.Text.Trim();
                string StrUpper = canshu.ToUpper();//转成大写
                string[] Str = new string[StrUpper.Length];//创建数组
                if (txtFilterBarcode.TextLength == 0 || txtFilterBarcode.TextLength < 26)
                {
                    //MessageBox.Show("请扫描二维码!");
                    if (txtFilterBarcode.TextLength != 0)
                    {
                        //分割条形码
                        Str[0] = StrUpper.Substring(0, 4);
                        Str[1] = StrUpper.Substring(4, 4);
                        Str[2] = StrUpper.Substring(9, 7);
                        Str[3] = StrUpper.Substring(17, 7);
                        //Str[4] = StrUpper.Substring(16, 1);
                        //Str[5] = StrUpper.Substring(17, 5);
                        //Str[6] = StrUpper.Substring(22, 5);
                        //Str[7] = StrUpper.Substring(27, 4);

                        //把截取的字符串赋值给textbox
                        txtFilterliaohao.Text = Str[2].ToString();
                        //txtdpVersion1.Text = Str[1].ToString();
                        txtFilterLayer.Text = Str[1].ToString();
                        //txtdpX1.Text = Str[3].ToString();
                        //txtdpY1.Text = Str[4].ToString();
                        //txtNum1.Text = Str[5].ToString();
                        //txtType1.Text = Str[6].ToString();
                        //txtTime1.Text = Str[7].ToString();

                        #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                        string sqlFilter = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
                        SqlParameter[] ps =
                        {
                            new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
                            new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
                        };

                        DataTable dt = SqlHelper.ExecuteTable(sqlFilter, ps);
                        this.dataGridView1.DataSource = dt;
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("请扫描条形码");
                    }
                }
                else
                {
                    MessageBox.Show("请扫描条形码");
                }
                #endregion
            }
        }

        private void BtnTakeOut_Click(object sender, EventArgs e)
        {
            if (!txtFilterBarcode.Text.Equals(""))
            {
                //判断是否已经有该编号的底片
                string sqlCheckNum = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
                SqlParameter[] para =
                {
                new SqlParameter("@dpliaohao",txtFilterliaohao.Text.Trim()),
                new SqlParameter("@dpLayer",txtFilterLayer.Text.Trim())
            };
                int dtNum = SqlHelper.ExecuteTable(sqlCheckNum, para).Rows.Count;

                if (dtNum == 1)
                {
                    //存在该编号底片则执行以下操作
                    //继续判断是否已经取出
                    string sqlCheckStatus = "select * from dipian_data where dpliaohao =@dpliaohao and dpLayer = @dpLayer and dpStatus = '取出'";
                    SqlParameter[] paraStatus =
                    {
                    new SqlParameter("@dpliaohao",txtFilterliaohao.Text.Trim()),
                    new SqlParameter("@dpLayer",txtFilterLayer.Text.Trim())
                };
                    int dtStatus = SqlHelper.ExecuteTable(sqlCheckStatus, paraStatus).Rows.Count;

                    if (dtStatus == 0)
                    {
                        DialogResult result = MessageBox.Show("请确认是否取出", "请筛选底片信息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                                {
                                    string selectedRow = Convert.ToString(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                                    //更新状态
                                    string sqlTakeOut = string.Format("update dipian_data set dpStatus = '取出', dpTakeOut =@dpTakeOut where dpliaohao = @dpliaohao and dpLayer = @dpLayer");
                                    SqlParameter[] psTakeout =
                                    {
                                        //new SqlParameter("@Version", txtFilterliaohao.Text.Trim()),
                                        new SqlParameter("@dpliaohao", txtFilterliaohao.Text.Trim()),
                                        new SqlParameter("@dpLayer", txtFilterLayer.Text.Trim()),
                                        //new SqlParameter("@X", txtdpX1.Text.Trim()),
                                        //new SqlParameter("@Y", txtdpY1.Text.Trim()),
                                        //new SqlParameter("@Num", txtNum1.Text.Trim()),
                                        //new SqlParameter("@Type", txtType1.Text.Trim()),
                                        //new SqlParameter("@Time", txtTime1.Text.Trim()),
                                        //new SqlParameter("@dpStorTime", txtStor.Text.Trim()),
                                        new SqlParameter("@dpTakeOut", DateTime.Now.ToLocalTime().ToString())
                                        //new SqlParameter("@dpStatus", txtStatus.Text.Trim()),
                                    };

                                    int s = Convert.ToInt32(SqlHelper.ExecuteNoneQuery(sqlTakeOut, psTakeout));
                                    if (s != 0)
                                    {
                                        MessageBox.Show("成功取出底片!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    #region 把总数量,存入数量,取出数量实时返回
                                    string sqlSum = "select * from dipian_data";//刷新datagridview
                                    DataTable dtSum = SqlHelper.ExecuteTable(sqlSum);//集合并生成表
                                    dataGridView1.DataSource = dtSum;//把生成表放入dgv

                                    string sqlStorSum = "select * from dipian_data where dpStatus = '存入'";
                                    string sqlTakeOutSum = "select * from dipian_data where dpStatus = '取出'";

                                    txtSaveSum.Text = SqlHelper.ExecuteTable(sqlStorSum).Rows.Count.ToString();
                                    txtTakeOutSum.Text = SqlHelper.ExecuteTable(sqlTakeOutSum).Rows.Count.ToString();
                                    txtSum.Text = dataGridView1.Rows.Count.ToString();
                                    #endregion
                                }
                                break;
                            case DialogResult.No:
                                break;
                        }

                        txtFilterBarcode.Focus();
                        txtFilterBarcode.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("仓库没有这个底片");
                }
            }
            else
            {
                MessageBox.Show("请扫描二维码");
            }
        }
    }
}

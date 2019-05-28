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
using System.Runtime.CompilerServices;

using System.Net;
using System.Runtime.InteropServices;

namespace Scan_01
{
    public partial class frCollect : Form
    {
        public frCollect()
        {
            InitializeComponent();
        }

        FinsTcp.PlcClient PLC = new FinsTcp.PlcClient();
        //PLC连接状态
        bool EntLink;
        long ScanCount;
        //DLL组件返回网络中PLC的连接句柄
        Int32 PlcHand;

        private void Form1_Load(object sender, EventArgs e)
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

            //2.PLC功能加载
            #region 连接PLC + PLC.EntLink("192.168.250.70", (Convert.ToUInt16("0")), "192.168.250.1", (Convert.ToUInt16("9600")), "DEMO", ref PlcHand);
            short re = 0;
            string restr = "需要重新连接PLC";
            re = PLC.EntLink("192.168.250.70", (Convert.ToUInt16("0")), "192.168.250.1", (Convert.ToUInt16("9600")), "DEMO", ref PlcHand);
            if (re == 0)
            {
                EntLink = true;
                MessageBox.Show("PLC联接成功!");
            }
            else
            {
                EntLink = false;
                MessageBox.Show("PLC联接失败: " + restr);
            }
            #endregion

        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAdd formAdd = new FormAdd();
            formAdd.Show();
        }

        private void BtnCheck_Click_1(object sender, EventArgs e)
        {
            #region 将barcode的字符串分割到料号,版本,层别,XY涨缩,编号,类型,周期中
            //字符分割并传入textbox
            string canshu = txtbarcode.Text.Trim();
            string StrUpper = canshu.ToUpper();//转成大写
            string[] Str = new string[StrUpper.Length];//将txtbarcode内容写进数组
            if (txtbarcode.TextLength == 0 || txtbarcode.TextLength < 26)
            {
                //MessageBox.Show("请扫描二维码!");
                if (txtbarcode.TextLength != 0)
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
                    txtdpliaohao1.Text = Str[2].ToString();
                    //txtdpVersion1.Text = Str[1].ToString();
                    txtdpLayer1.Text = Str[1].ToString();
                    //txtdpX1.Text = Str[3].ToString();
                    //txtdpY1.Text = Str[4].ToString();
                    //txtNum1.Text = Str[5].ToString();
                    //txtType1.Text = Str[6].ToString();
                    //txtTime1.Text = Str[7].ToString();
                    

                    #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                    string sql = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
                    SqlParameter[] ps =
                    {
                        new SqlParameter("@dpliaohao", txtdpliaohao1.Text.Trim()),
                        new SqlParameter("@dpLayer", txtdpLayer1.Text.Trim()),
                    };

                    DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                    this.dataGridView1.DataSource = dt;
                    #endregion
                }
                else
                {
                    MessageBox.Show("请扫描二维码");
                }
            }
            else
            {
                Str[0] = StrUpper.Substring(0, 7);
                Str[1] = StrUpper.Substring(7, 4);
                Str[2] = StrUpper.Substring(11, 4);
                Str[3] = StrUpper.Substring(15, 1);
                Str[4] = StrUpper.Substring(16, 1);
                Str[5] = StrUpper.Substring(17, 5);
                Str[6] = StrUpper.Substring(22, 5);
                Str[7] = StrUpper.Substring(27, 4);

                //把截取的字符串赋值给料号,版本,层别,编号,XY涨缩,编号,类型,周期中
                txtdpliaohao1.Text = Str[0].ToString();
                txtdpVersion1.Text = Str[1].ToString();
                txtdpLayer1.Text = Str[2].ToString();
                txtdpX1.Text = Str[3].ToString();
                txtdpY1.Text = Str[4].ToString();
                txtNum1.Text = Str[5].ToString();
                txtType1.Text = Str[6].ToString();
                txtTime1.Text = Str[7].ToString();

                //txtStor.Text = DateTime.Now.ToLocalTime().ToString();//获取存入当前时间
                //txtTakeOut.Text = DateTime.Now.ToLocalTime().ToString();//获取取出时间
            }
            #endregion
        }

        private void BtnDelete_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请确认是否删除", "警告：操作需谨慎。本条资料一旦删除将不可恢复！", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (result)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                    {
                        string selectedRow = Convert.ToString(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                        //string name = Convert.ToString(mainView.SelectedRows[i - 1].Cells[1].Value);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);

                        //使用获得的selectRow删除数据库的数据
                        string sql = string.Format("delete from dipian_data where Id='{0}'", selectedRow);
                        int s = Convert.ToInt32(SqlHelper.ExecuteNoneQuery(sql)); 
                        if (s != 0)
                        {
                            MessageBox.Show("成功删除选中行数据!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        #region 清空输入textbox
                        //清空输入textbox
                        txtbarcode.Text = "";
                        txtdpVersion1.Text = "";
                        txtdpliaohao1.Text = "";
                        txtdpX1.Text = "";
                        txtdpY1.Text = "";
                        txtNum1.Text = "";
                        txtType1.Text = "";
                        txtTime1.Text = "";
                        txtbarcode.Focus();
                        #endregion
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void BtnAdd2_Click_1(object sender, EventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //string sql = "select * from dipian_data";

            //#region XY涨缩的联合查询+枚举
            //if (txtSizing_X1.Text.Trim() != "")//第一个txtX1不为空
            //{
            //    if (txtSizing_X2.Text.Trim() != "")//第二个txtX2不为空
            //    {
            //        sql = sql + " where (dpx>@dpX1 and dpX <@dpX2 ) ";

            //        if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " and (dpY>@dpY1 and dpY <@dpY2 ) ";
            //            }
            //            else//第二个txtY2为空
            //            {
            //                sql = sql + " and (dpY>@dpY1 ) ";
            //            }
            //        }
            //        else//第一个txtY1为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " and ( dpY <@dpY2 ) ";
            //            }
            //        }
            //    }
            //    else//第二个txtX2为空
            //    {
            //        sql = sql + " where (dpx>@dpX1) ";

            //        if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " and (dpY>@dpY1 and dpY <@dpY2 ) ";
            //            }
            //            else//第二个txtY2为空
            //            {
            //                sql = sql + " and (dpY>@dpY1 ) ";
            //            }
            //        }
            //        else
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " and ( dpY <@dpY2 ) ";
            //            }
            //        }
            //    }
            //}
            //else//第一个txtX1为空
            //{
            //    if (txtSizing_X2.Text.Trim() != "")//第二个txtX2不为空
            //    {
            //        sql = sql + " where ( dpX <@dpX2 ) ";

            //        if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " and (dpY>@dpY1 and dpY <@dpY2 ) ";
            //            }
            //            else//第二个txtY2为空
            //            {
            //                sql = sql + " and (dpY>@dpY1 ) ";
            //            }
            //        }
            //        else//第一个txtY1为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " and ( dpY <@dpY2 ) ";
            //            }
            //        }
            //    }
            //    else//第二个txtX2为空
            //    {
            //        if (txtSizing_Y1.Text.Trim() != "")//第一个txtY1不为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " where (dpY>@dpY1 and dpY <@dpY2 ) ";
            //            }
            //            else//第二个txtY2为空
            //            {
            //                sql = sql + " where (dpY>@dpY1 ) ";
            //            }
            //        }
            //        else//第一个txtY1为空
            //        {
            //            if (txtSizing_Y2.Text.Trim() != "")//第二个txtY2不为空
            //            {
            //                sql = sql + " where ( dpY <@dpY2 ) ";
            //            }
            //        }
            //    }
            //}
            //#endregion

            //#region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //SqlParameter[] ps =
            //{
            //    new SqlParameter("@dpX1", txtSizing_X1.Text.Trim()),
            //    new SqlParameter("@dpX2", txtSizing_X2.Text.Trim()),
            //    new SqlParameter("@dpY1", txtSizing_Y1.Text.Trim()),
            //    new SqlParameter("@dpY2", txtSizing_Y2.Text.Trim()),
            //};

            //DataTable dt = SqlHelper.ExecuteTable(sql, ps);
            //this.dataGridView1.DataSource = dt;
            //#endregion

        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            #region 将超过寿命的底片标红
            if (e.Value == null)
                return;

            DateTime dpStor = Convert.ToDateTime(this.dataGridView1.Rows[e.RowIndex].Cells["Column10"].Value);
            if (!(this.dataGridView1.Rows[e.RowIndex].Cells["Column11"].Value.ToString() == ""))
            {
                DateTime dpTakeOut = Convert.ToDateTime(this.dataGridView1.Rows[e.RowIndex].Cells["Column11"].Value);

                TimeSpan Ts1 = dpStor - dpTakeOut;
                TimeSpan Ts2 = dpTakeOut - dpStor;

                int days1 = Ts1.Days;
                int days2 = Ts2.Days;

                if (dataGridView1.Columns[e.ColumnIndex].Name == "Column13" && (System.Math.Abs(days1 - days2) > 20))
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }
            #endregion
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            #region 建立和筛选窗口的连接
            FrmFilter frmFilter = new FrmFilter();
            frmFilter.Show();
            #endregion
        }

        private void BtnFilter_Click_1(object sender, EventArgs e)
        {
            #region 建立和筛选窗口的连接
            FrmFilter frmFilter = new FrmFilter();
            frmFilter.Show();
            #endregion
        }

        private void BtnTakeOut_Click(object sender, EventArgs e)
        {
            #region 字符分割
            string canshu = txtbarcode.Text.Trim();
            string StrUpper = canshu.ToUpper();//转成大写
            string[] Str = new string[StrUpper.Length];//创建数组

            //判断二维码还是条形码
            if (txtbarcode.TextLength == 0 || txtbarcode.TextLength < 26)
            {
                if (txtbarcode.TextLength != 0)
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
                    txtdpliaohao1.Text = Str[2].ToString();
                    //txtdpVersion1.Text = Str[1].ToString();
                    txtdpLayer1.Text = Str[1].ToString();
                    //txtdpX1.Text = Str[3].ToString();
                    //txtdpY1.Text = Str[4].ToString();
                    //txtNum1.Text = Str[5].ToString();
                    //txtType1.Text = Str[6].ToString();
                    //txtTime1.Text = Str[7].ToString();

                    #region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                    string sql = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
                    SqlParameter[] ps =
                    {
                        new SqlParameter("@dpliaohao", txtdpliaohao1.Text.Trim()),
                        new SqlParameter("@dpLayer", txtdpLayer1.Text.Trim()),
                    };

                    DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                    this.dataGridView1.DataSource = dt;
                    #endregion
                }
                else
                {
                    MessageBox.Show("请扫描二维码");
                }
            }
            else
            {
                Str[0] = StrUpper.Substring(0, 7);
                Str[1] = StrUpper.Substring(7, 4);
                Str[2] = StrUpper.Substring(11, 4);
                Str[3] = StrUpper.Substring(15, 1);
                Str[4] = StrUpper.Substring(16, 1);
                Str[5] = StrUpper.Substring(17, 5);
                Str[6] = StrUpper.Substring(22, 5);
                Str[7] = StrUpper.Substring(27, 4);

                //把截取的字符串赋值给textbox
                txtdpliaohao1.Text = Str[0].ToString();
                txtdpVersion1.Text = Str[1].ToString();
                txtdpLayer1.Text = Str[2].ToString();
                txtdpX1.Text = Str[3].ToString();
                txtdpY1.Text = Str[4].ToString();
                txtNum1.Text = Str[5].ToString();
                txtType1.Text = Str[6].ToString();
                txtTime1.Text = Str[7].ToString();

                //txtStor.Text = DateTime.Now.ToLocalTime().ToString();//获取存入当前时间
                //txtTakeOut.Text = DateTime.Now.ToLocalTime().ToString();//获取取出时间
            }
            #endregion

            //判断是否已经有该编号的底片
            string sqlCheckNum = "select * from dipian_data where dpNum = @Num";
            SqlParameter[] para =
            {
                new SqlParameter("@Num",txtNum1.Text.Trim())
            };
            int dtNum = SqlHelper.ExecuteTable(sqlCheckNum, para).Rows.Count;

            if (dtNum == 1)
            {
                //存在该编号底片则执行以下操作
                //继续判断是否已经取出
                string sqlCheckStatus = "select * from dipian_data where dpNum = @Num and dpStatus = '取出'";
                SqlParameter[] paraStatus =
                {
                    new SqlParameter("@Num",txtNum1.Text.Trim())
                };
                int dtStatus = SqlHelper.ExecuteTable(sqlCheckStatus, paraStatus).Rows.Count;

                if (dtStatus == 0)
                {
                    //切换状态到取出,获取取出时间
                    txtStatus.Text = "取出";
                    txtTakeOut.Text = DateTime.Now.ToLocalTime().ToString();
                    string sql = "update dipian_data set dpStatus = '取出', dpTakeOut = @dpTakeOut where dpNum = @Num";
                    SqlParameter[] ps =
                    {
                        //new SqlParameter("@Version", txtdpVersion1.Text.Trim()),
                        //new SqlParameter("@liaohao", txtdpliaohao1.Text.Trim()),
                        //new SqlParameter("@Layer", txtdpLayer1.Text.Trim()),
                        //new SqlParameter("@X", txtdpX1.Text.Trim()),
                        //new SqlParameter("@Y", txtdpY1.Text.Trim()),
                        new SqlParameter("@Num", txtNum1.Text.Trim()),
                        //new SqlParameter("@Type", txtType1.Text.Trim()),
                        //new SqlParameter("@Time", txtTime1.Text.Trim()),
                        //new SqlParameter("@dpStorTime", txtStor.Text.Trim()),
                        new SqlParameter("@dpTakeOut", DateTime.Now.ToLocalTime().ToString()),
                        new SqlParameter("@dpStatus", txtStatus.Text.Trim()),
                    };

                    if (SqlHelper.ExecuteNoneQuery(sql, ps) == 1)
                    {
                        sql = "select * from dipian_data";//刷新datagridview
                        DataTable dt = SqlHelper.ExecuteTable(sql);//集合并生成表
                        dataGridView1.DataSource = dt;//把生成表放入dgv
                        txtSum.Text = dataGridView1.Rows.Count.ToString();
                    }

                    txtbarcode.Focus();
                    txtbarcode.Text = "";
                }
            }
            else
            {
                MessageBox.Show("仓库没有这个底片");
            }
         }

        private void FrCollect_Activated(object sender, EventArgs e)
        {
            //窗口激活触发的事件:
            //1.初始化焦点
            txtbarcode.Focus();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 建立和位置状态窗口的连接
            FmAddress fmAddress = new FmAddress();
            fmAddress.Show();
            #endregion
        }

        private void Txtbarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //字符分割并传入textbox
                string canshu = txtbarcode.Text.Trim();
                string StrUpper = canshu.ToUpper();//转成大写
                string[] Str = new string[StrUpper.Length];//创建大写字符数组

                if (txtbarcode.TextLength == 0 || txtbarcode.TextLength < 26)
                {
                    //分割条形码
                    //Str[0] = StrUpper.Substring(0, 4);
                    //Str[1] = StrUpper.Substring(4, 4);
                    //Str[2] = StrUpper.Substring(9, 7);
                    //Str[3] = StrUpper.Substring(17, 7);
                    ////Str[4] = StrUpper.Substring(16, 1);
                    ////Str[5] = StrUpper.Substring(17, 5);
                    ////Str[6] = StrUpper.Substring(22, 5);
                    ////Str[7] = StrUpper.Substring(27, 4);

                    ////把截取的字符串赋值给textbox
                    //txtdpliaohao1.Text = Str[2].ToString();
                    ////txtdpVersion1.Text = Str[1].ToString();
                    //txtdpLayer1.Text = Str[1].ToString();
                    ////txtdpX1.Text = Str[3].ToString();
                    ////txtdpY1.Text = Str[4].ToString();
                    ////txtNum1.Text = Str[5].ToString();
                    ////txtType1.Text = Str[6].ToString();
                    ////txtTime1.Text = Str[7].ToString();

                    //#region 将表格返回到datagridview+DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                    //string sql = "select * from dipian_data where dpliaohao = @dpliaohao and dpLayer = @dpLayer";
                    //SqlParameter[] ps =
                    //{
                    //    new SqlParameter("@dpliaohao", txtdpliaohao1.Text.Trim()),
                    //    new SqlParameter("@dpLayer", txtdpLayer1.Text.Trim()),
                    //};

                    //DataTable dt = SqlHelper.ExecuteTable(sql, ps);
                    //this.dataGridView1.DataSource = dt;
                    //#endregion

                    MessageBox.Show("请不要在输入窗口输入条形码.");
                }
                else
                {
                    #region 分割字符
                    //分割字符
                    Str[0] = StrUpper.Substring(0, 7);
                    Str[1] = StrUpper.Substring(7, 4);
                    Str[2] = StrUpper.Substring(11, 4);
                    Str[3] = StrUpper.Substring(15, 1);
                    Str[4] = StrUpper.Substring(16, 1);
                    Str[5] = StrUpper.Substring(17, 5);
                    Str[6] = StrUpper.Substring(22, 5);
                    Str[7] = StrUpper.Substring(27, 4);

                    //把截取的字符串赋值给textbox
                    txtdpliaohao1.Text = Str[0].ToString();
                    txtdpVersion1.Text = Str[1].ToString();
                    txtdpLayer1.Text = Str[2].ToString();
                    txtdpX1.Text = Str[3].ToString();
                    txtdpY1.Text = Str[4].ToString();
                    txtNum1.Text = Str[5].ToString();
                    txtType1.Text = Str[6].ToString();
                    txtTime1.Text = Str[7].ToString();
                    #endregion

                    //判断是否存在该编号的底片
                    string sqlCheckNum = "select * from dipian_data where dpNum = @Num";
                    SqlParameter[] para =
                    {
                        new SqlParameter("@Num",txtNum1.Text.Trim())
                    };
                    int dtNum = SqlHelper.ExecuteTable(sqlCheckNum, para).Rows.Count;

                    //存在该编号的底片
                    if (dtNum == 1)
                    {
                        //继续判断是否已经取出
                        string sqlCheckStatus = "select * from dipian_data where dpNum = @Num and dpStatus = '存入'";
                        SqlParameter[] paraStatus =
                        {
                            new SqlParameter("@Num",txtNum1.Text.Trim())
                        };

                        int dtStatus = SqlHelper.ExecuteTable(sqlCheckStatus, paraStatus).Rows.Count;
                        if (dtStatus == 0)
                        {
                            //切换状态到取出,获取取出时间
                            #region 更新底片信息
                            string sql = "update dipian_data set dpStatus = '取出', dpTakeOut = @dpTakeOut where dpNum = @Num";
                            SqlParameter[] ps =
                            {
                                //new SqlParameter("@Version", txtdpVersion1.Text.Trim()),
                                //new SqlParameter("@liaohao", txtdpliaohao1.Text.Trim()),
                                //new SqlParameter("@Layer", txtdpLayer1.Text.Trim()),
                                //new SqlParameter("@X", txtdpX1.Text.Trim()),
                                //new SqlParameter("@Y", txtdpY1.Text.Trim()),
                                new SqlParameter("@Num", txtNum1.Text.Trim()),
                                //new SqlParameter("@Type", txtType1.Text.Trim()),
                                //new SqlParameter("@Time", txtTime1.Text.Trim()),
                                //new SqlParameter("@dpStorTime", txtStor.Text.Trim()),
                                new SqlParameter("@dpTakeOut", DateTime.Now.ToLocalTime().ToString()),
                                new SqlParameter("@dpStatus", "存入"),
                            };
                            if (SqlHelper.ExecuteNoneQuery(sql, ps) == 1)
                            {
                                sql = "select * from dipian_data";//刷新datagridview
                                DataTable dt = SqlHelper.ExecuteTable(sql);//集合并生成表
                                dataGridView1.DataSource = dt;//把生成表放入dgv
                                txtSum.Text = dataGridView1.Rows.Count.ToString();
                            }
                            #endregion

                            //存入完成后的操作:
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
                            #region 清空输入textbox
                            //清空输入textbox
                            txtbarcode.Text = "";
                            txtdpVersion1.Text = "";
                            txtdpliaohao1.Text = "";
                            txtdpX1.Text = "";
                            txtdpY1.Text = "";
                            txtNum1.Text = "";
                            txtType1.Text = "";
                            txtTime1.Text = "";
                            txtbarcode.Focus();
                            #endregion

                        }
                        else
                        {
                            MessageBox.Show("该底片已经存入.");
                        }
                    }
                    else
                    {
                        //不存在该编号的底片则直接写入信息
                        #region 存入底片信息
                        string sql = "insert into dipian_data (dpVersion, dpliaohao, dpLayer, dpX, dpY, dpNum, dpType, dpTime, dpStorTime, dpTakeOut, dpStatus) values(@Version, @liaohao, @Layer, @X, @Y, @Num, @Type, @Time, @dpStorTime, @dpTakeOut, @dpStatus) ";
                        SqlParameter[] ps =
                        {
                            new SqlParameter("@Version", txtdpVersion1.Text.Trim()),
                            new SqlParameter("@liaohao", txtdpliaohao1.Text.Trim()),
                            new SqlParameter("@Layer", txtdpLayer1.Text.Trim()),
                            new SqlParameter("@X", txtdpX1.Text.Trim()),
                            new SqlParameter("@Y", txtdpY1.Text.Trim()),
                            new SqlParameter("@Num", txtNum1.Text.Trim()),
                            new SqlParameter("@Type", txtType1.Text.Trim()),
                            new SqlParameter("@Time", txtTime1.Text.Trim()),
                            new SqlParameter("@dpStorTime", DateTime.Now.ToLocalTime().ToString()),
                            new SqlParameter("@dpTakeOut", txtTakeOut.Text.Trim()),
                            new SqlParameter("@dpStatus", "存入"),
                        };
                        if (SqlHelper.ExecuteNoneQuery(sql, ps) == 1)
                        {
                            sql = "select * from dipian_data";//刷新datagridview
                            DataTable dt = SqlHelper.ExecuteTable(sql);//集合并生成表
                            dataGridView1.DataSource = dt;//把生成表放入dgv
                            txtSum.Text = dataGridView1.Rows.Count.ToString();
                        }
                        #endregion

                        //存入完成的操作:
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
                        #region 清空输入textbox
                        //清空输入textbox
                        txtbarcode.Text = "";
                        txtdpVersion1.Text = "";
                        txtdpliaohao1.Text = "";
                        txtdpX1.Text = "";
                        txtdpY1.Text = "";
                        txtNum1.Text = "";
                        txtType1.Text = "";
                        txtTime1.Text = "";
                        txtbarcode.Focus();
                        #endregion
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLog frmLog = new FrmLog();
            frmLog.Show();
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    string intGrade = this.dataGridView1.Rows[e.RowIndex].Cells["Column13"].Value.ToString();
            //    if (intGrade == "存入")
            //    {
            //        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}
        }
    }
 }

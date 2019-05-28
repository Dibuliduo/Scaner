using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO.Ports;
using System.Threading;
namespace DieCutting
{

    class MySerialPorts
    {
        string receive = null;
        public SerialPort Comports = new SerialPort();
        string[] str;
        string strRcv = null;
        public string result = null;
        List<byte> buffer = new List<byte>(4096);
        public bool OpenComPortSig = false;
        public bool ComportSendingSig = false;

        public void GetComports()
        {
            str = SerialPort.GetPortNames();
            if (str == null)
            {
                Console.Write("找不到端口");
                Console.ReadLine();
                return;
            }

        }
        public void CloseComports()
        {
            Comports.Close();
            OpenComPortSig = false;
        }
        public void OpenComports()
        {
            GetComports();
            if (!Comports.IsOpen)
            {
                try
                {
                    //设置串口号
                    string serialName = str[0];
                    Comports.PortName = serialName;

                    //设置各“串口设置”
                    Int32 iBaudRate = Convert.ToInt32("57600");
                    Int32 iDateBits = Convert.ToInt32("7");

                    Comports.BaudRate = iBaudRate;       //波特率
                    Comports.DataBits = iDateBits;       //数据位
                    Comports.StopBits = StopBits.Two;   //停止位
                    Comports.Parity = Parity.Even;           //校验位
                    Comports.ReadTimeout = 2000;
                    Comports.WriteBufferSize = 1024;
                    Comports.ReadBufferSize = 1024;
                    Comports.RtsEnable = true;
                    Comports.DtrEnable = true;
                    Comports.ReceivedBytesThreshold = 1;
                    Comports.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    //OpenComPortSig = true;

                }
                catch (System.Exception ex)
                {
                    OpenComPortSig = false;
                    Console.Write(ex);
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                OpenComPortSig = true;
                //Comports.Close();     //打开串口
                Console.Write("已经被打开");
               
            }
            try 
            {
                Comports.Open();
                OpenComPortSig = true;
            }
            catch (System.Exception ex)
            {
                OpenComPortSig = false;
                Console.Write(ex);
                //return;
            }
        }



        /*public void SendCommand(String Commands, String Address, String Data)
        {
            string FCS;
            string input = "00" + Commands + Address + Data;
            char[] values = input.ToCharArray();
            char[] first = "@".ToCharArray();
            int sum = first[0];
            foreach (char letter in values)
            {
                sum = letter ^ sum;
            }
            FCS = Convert.ToString(sum, 16);
            string sendcommand = "@00" + Commands + Address + Data + FCS + "*\r\n";
            Comports.WriteLine(sendcommand);    //写入数据
        }*/
        private int CommandTrant2(String Commands, String Address, String Data)
        {
            try
            {
                //FCS转换
                string FCS;
                int temp;
                temp = Convert.ToInt32(Data);
                Data = Convert.ToString(temp, 16);
                Data = Data.PadLeft(4, '0');
                Commands = Commands.ToUpper();
                Address = Address.ToUpper();
                Data = Data.ToUpper();  
                string input = "00" + Commands + Address + Data;
                char[] values = input.ToCharArray();
                char[] first = "@".ToCharArray();
                int sum = first[0];
                foreach (char letter in values)
                {
                    sum = letter ^ sum;
                }
                FCS = Convert.ToString(sum, 16);
                
                //发送指令
                string sendcommand = "@00" + Commands + Address + Data + FCS.ToUpper()+"*\r\n";
                //Console.Write(sendcommand);

                string FCS2;
                string input2 = "00" + Commands + "00";
                char[] values2 = input2.ToCharArray();
                char[] first2 = "@".ToCharArray();
                int sum2 = first2[0];
                foreach (char letter2 in values2)
                {
                    sum2 = letter2 ^ sum2;
                }
                FCS2 = Convert.ToString(sum2, 16);
                Comports.WriteLine(sendcommand);
                result = null;
                Thread.Sleep(70);
                Console.Write("正确输出："+"@00" + Commands + "00" + FCS2.ToUpper() + "*\n");
                int count = 0;
                while (result != "@00" + Commands + "00" + FCS2.ToUpper() + "*")
                {
                    if(result=="@00WD0152*")
                    {
                        return 0;
                    }
                    
                    //Console.Write("实际输出：" + result);
                    Comports.WriteLine(sendcommand);
                    //Thread.Sleep(20);
                    count++;
                    if (count == 3)
                    {
                        OpenComPortSig = false;
                        return -1;
                    }
                    
                   // Console.Write("输入命令：" + sendcommand + "\n");
                }
                return 0;
                //Console.Write("最后输出：" + result);
            }
            catch
            {
                return -1;
            }
        }
        public int CommandTrant(String Commands, String Address, String Data)
        {
            int SendOK;
            //string Commandsbegin = "SC";
            //string Addressbegin = "0";
            //string Databegin = "2";
            //OpenComports();  
            ComportSendingSig = true;
          //  Comports.WriteLine("@00SC0252*\r\n");
            SendOK=CommandTrant2(Commands, Address, Data);
            //string CommandsEnd = "SC";
            //string AddressEnd = "0";
            //string DataEnd = "3";
           // Comports.WriteLine("@@00SC0353*\r\n");
           // CloseComports();  
            ComportSendingSig = false;

            return SendOK;
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(50);
            try
            {
               
                //serialPort1.BytesToRead
                byte[] readBuffer = null;
                int n = Comports.BytesToRead;

                byte[] buf = new byte[n];
                Comports.Read(buf, 0, n);
                //1.缓存数据           
                buffer.AddRange(buf);
                //label1.Text = buf.ToString();
                //2.完整性判断         
                while (buffer.Count >= 10)
                {
                    //至少包含标头(1字节),长度(1字节),校验位(2字节)等等
                    //2.1 查找数据标记头            
                    if (buffer[0] == 0x40) //传输数据有帧头，用于判断       
                    {

                        readBuffer = new byte[10];
                        //得到完整的数据，复制到readBuffer中    
                        buffer.CopyTo(0, readBuffer, 0, 10);
                        result = System.Text.Encoding.Default.GetString(readBuffer);
                        //label1.Text = result;
                        //从缓冲区中清除
                        buffer.RemoveRange(0, 10);


                        //触发外部处理接收消息事件

                    }
                    else //开始标记或版本号不正确时清除           
                    {

                        buffer.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("1错" + ex);
            }

            /*serialReadString = myport.Comports.ReadExisting();
            label1.Text = serialReadString;*/
        }
    }

}

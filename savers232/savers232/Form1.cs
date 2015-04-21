using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;

namespace savers232
{


    public partial class Form1 : Form
    {
        public SerialPort serialPort;
        public FileStream fs;
        public StreamWriter sr;
        private Thread th;

        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            //textBox1.Text = "18";
            textBox2.Text = "25";

            string[] ports = SerialPort.GetPortNames();
           // Array.Sort(ports);
            comboBox2.Items.AddRange(ports);
            if(ports.Length > 0)
                comboBox2.SelectedIndex = 0;
        }


        public void AddText(int _str)
        {
            this.richTextBox1.Text += Convert.ToChar(_str);
        }

        public void AddText(string _str)
        {
            this.richTextBox1.Text += _str;
        }

        private int GetReadTimeout()
        {
            int timeout = 25;
            try
            {
                timeout = Convert.ToInt32(textBox2.Text);
            }
            catch(Exception ee)
            {
            }

            return timeout;
        }
 
        private Parity GetParity()
        {
            Parity ret = Parity.None;

            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    ret = Parity.None;
                    break;
                case 1:
                    ret = Parity.Odd;
                    break;
                case 2:
                    ret = Parity.Even;
                    break;
                case 3:
                    ret = Parity.Space;
                    break;
                case 4:
                    ret = Parity.Mark;
                    break;
                default:
                    ret = Parity.None;
                    break;
                 
            }

            return ret;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //открытие COM порта
                serialPort = new SerialPort();
                serialPort.PortName = comboBox2.Text.Trim();
                serialPort.BaudRate = 9600;
                serialPort.Parity = GetParity();

                serialPort.Open();
                richTextBox1.Text += "Port " + comboBox2.Text.Trim() + " is open\n";

                fs = new FileStream(@"c:\tmp\mdb.log", FileMode.Append,FileAccess.Write);

                richTextBox1.Text += "File " + @"c:\tmp\mdb.log" + " is open\n";

                richTextBox1.Text += "Creating thread\n";

                th = new Thread(Work.DoWork);
                th.Start(this);

                richTextBox1.Text += "Thread was created\n";


            }
            catch(Exception ex)
            {
                richTextBox1.Text += "\n Exception: " + ex.Message;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Work.cont = false;
            this.Cursor = Cursors.WaitCursor;
            Thread.Sleep(500);
            this.Cursor = Cursors.Arrow;

            try { th.Abort(); }
            catch (Exception tt) { }
            
            richTextBox1.Text += "\nThread closed\n";

            try { fs.Close(); }catch (Exception tt){}
            richTextBox1.Text += "File closed\n";

            try
            {
                if (serialPort != null && serialPort.IsOpen)
                    serialPort.Close();
            }
            catch (Exception tt) { }

            richTextBox1.Text += "Port closed\n";

        }

        private void button3_Click(object sender, EventArgs e)
        {
 
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            ASCIIEncoding ascii = new ASCIIEncoding();
            //Byte[] encodedBytes = ascii.GetBytes(unicodeString);
            //String decodedString = ascii.GetString(encodedBytes);

            byte[] in_buff = new byte[512];
            in_buff[0] = (byte)'1';
            in_buff[1] = (byte)'2';
            in_buff[2] = (byte)'3';
            in_buff[3] = (byte)'6';
            in_buff[4] = (byte)'t';

                

            String str = ascii.GetString(in_buff, 0, 5);
            richTextBox1.Text += "string" + str;

        }
        
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

                //открытие COM порта
                serialPort = new SerialPort();
                serialPort.PortName = comboBox2.Text.Trim();
                serialPort.BaudRate = 9600;
                serialPort.Parity = GetParity();

                serialPort.Open();
                richTextBox1.Text += "Port " + comboBox2.Text.Trim() + " is open\n";

                fs = new FileStream(@"c:\tmp\mdb.log", FileMode.Append, FileAccess.Write);

                richTextBox1.Text += "File " + @"c:\tmp\mdb.log" + " is open\n";

                richTextBox1.Text += "Creating thread\n";

                th = new Thread(Work.DoWorkCommand);
                th.Start(this);

                richTextBox1.Text += "Thread was created\n";


            }
            catch (Exception ex)
            {
                richTextBox1.Text += "\n Exception: " + ex.Message;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public class Work
    {
        public static Boolean cont = true;

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        // public static int ThreadProc(Form1 _form) 
        // test comment
        public static void DoWork(object data)
        {

            cont = true;
            do
            {
               
                if (((Form1)data).serialPort.BytesToRead > 0)
                {
                    int rbyte = ((Form1)data).serialPort.ReadByte();
                    ((Form1)data).AddText(rbyte);
                    ((Form1)data).fs.WriteByte((byte)rbyte);
                }
                Thread.Sleep(10);
            } while (cont);

        }

        // public static int ThreadProc(Form1 _form)
        public static void DoWorkCommand(object data)
        {
            byte[] in_buff = new byte[512];
            int read_size=0;

            cont = true;
            do
            {
                read_size = 0;
                if (((Form1)data).serialPort.BytesToRead > 0)
                {
                    read_size = ((Form1)data).serialPort.Read(in_buff, 0, 512);
                    if (read_size > 0)
                    {
                        String str;
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        str = ascii.GetString(in_buff, 0, read_size);

                        ((Form1)data).AddText("Command - " + str + "\n");
                        ((Form1)data).fs.Write(in_buff,0,read_size);
                    }
               }
               Thread.Sleep(70);
            } while (cont);

        }
    }

}

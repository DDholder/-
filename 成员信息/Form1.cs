using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 成员信息
{
    public partial class Form1 : Form
    {
        string strConnection;
        SqlDataAdapter adapter;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“memberDataSet2.member”中。您可以根据需要移动或删除它。
            this.memberTableAdapter1.Fill(this.memberDataSet2.member);
            memberDataSet2.EnforceConstraints = false;
            try
            {
                strConnection = "Server=(localdb)\\ProjectsV13;";//选择服务器
                strConnection += "Initial Catalog=member;";      //选择表
                strConnection += "User Id=ddd;";                 //用户名
                strConnection += "Password=ddd;";                //密码
                strConnection += "Connect Timeout=5";            //超时
                adapter = new SqlDataAdapter("select * from member", strConnection);
                SqlConnection conn = new SqlConnection(strConnection);
                SqlCommand SCD = new SqlCommand("select * from member ", conn);
                adapter.SelectCommand = SCD;
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter);
            try
            {
                adapter.Update(memberDataSet2, "member");
                this.memberTableAdapter1.Fill(this.memberDataSet2.member);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.memberTableAdapter1.Fill(this.memberDataSet2.member);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string str = ("select * from member where ID=" + textBox1.Text);
                SqlConnection conn = new SqlConnection(strConnection);
                SqlCommand SCD = new SqlCommand(str, conn);
                conn.Open();
                SqlDataReader reder = SCD.ExecuteReader();
                if (reder.Read())
                    label1.Text = reder["电话"].ToString();
                else
                    MessageBox.Show("no this message");
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string portRead = serialPort1.ReadLine();
            Action showReceive = () =>
            {
                textBox1.Text= portRead;
            };
            this.Invoke(showReceive);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                    button4.Text = "打开串口";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    button4.Text = "关闭串口";
                }
            }
            else
            {
                try
                {
                    serialPort1.PortName = comboBoxSerialPortName.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBoxBaudRate.Text);
                    serialPort1.Open();
                    button4.Text = "关闭串口";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    button4.Text = "打开串口";
                }
            }
        }
        private void timerscanserial_Tick(object sender, EventArgs e)
        {
            scanSerial();
        }
        int portNamesLength = 0;
        void scanSerial()
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames();
                if (portNames.Length != portNamesLength)
                {
                    comboBoxSerialPortName.Items.Clear();
                    comboBoxSerialPortName.Items.AddRange(portNames);
                    comboBoxSerialPortName.SelectedIndex = 0;
                    portNamesLength = portNames.Length;
                }
            }
            catch//(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void 刷新数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.memberTableAdapter1.Fill(this.memberDataSet2.member);
        }

        private void 保存更改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter);
            try
            {
                adapter.Update(memberDataSet2, "member");
                this.memberTableAdapter1.Fill(this.memberDataSet2.member);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string str = ("select * from member where ID=" + textBox1.Text);
                SqlConnection conn = new SqlConnection(strConnection);
                SqlCommand SCD = new SqlCommand(str, conn);
                conn.Open();
                SqlDataReader reder = SCD.ExecuteReader();
                if (reder.Read())
                {
                    textBox2.Text = reder["姓名"].ToString();
                    textBox3.Text = reder["学号"].ToString();
                    textBox4.Text = reder["电话"].ToString();
                }
                else
                    MessageBox.Show("no this message");
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

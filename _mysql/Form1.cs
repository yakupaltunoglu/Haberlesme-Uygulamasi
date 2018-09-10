using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO.Ports;
using System.Windows.Forms;

namespace _mysql
{
    public partial class Form1 : Form
    {
        string[] ports = SerialPort.GetPortNames();
        public Form1()
        {
            InitializeComponent();
            serialPort1.BaudRate = 9600;
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                comboBox1.SelectedIndex = 0;
            }
        }
        db _vt = new db();
        private void Form1_Load(object sender, EventArgs e)
        {
            if (_vt.baglanti_kontrol() == "true")
            {
                MessageBox.Show("Veritabanı bağlantisi kuruldu");
               
                
            }
            else
            {
                MessageBox.Show(_vt.baglanti_kontrol());
            }
        }

        private void btn_baglan_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (comboBox1.Text == "")
                    return;
                serialPort1.PortName = comboBox1.Text;
                try
                {
                    serialPort1.Open();
                    MessageBox.Show("Bağlantı Başarıyla Kuruldu.");
                    timer1.Start();
                }
                catch (Exception ex)
                {
                    timer1.Stop();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            
            if (serialPort1.IsOpen == true) { 
            serialPort1.Close();
            MessageBox.Show("Bağlantı sonlandırıldı.");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
                {
                string sonuc = serialPort1.ReadLine();
                string[] sensorler = sonuc.Split('*');
               

                textBox1.Text = sensorler[0];
                textBox2.Text = sensorler[1];
                textBox3.Text = sensorler[2];
                textBox4.Text = sensorler[3];
                serialPort1.DiscardInBuffer();
                if (_vt.baglanti.State == ConnectionState.Closed)
                {
                    _vt.baglanti.Open();
                }
                MySqlCommand komut = new MySqlCommand("INSERT INTO sensor_tablosus (sensor1,sensor2,sensor3,sensor4) VALUES (" + textBox1.Text + ",'" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "')", _vt.baglanti);
                komut.ExecuteNonQuery();
                _vt.baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _vt.baglanti.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            serialPort1.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

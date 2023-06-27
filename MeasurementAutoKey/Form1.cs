using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeasurementAutoKey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HideSetting(1);
            findarduino();
        }
        delegate void serialCalback(string val);
        bool btnHide = false;
        private void btnhideSetting_Click(object sender, EventArgs e)
        {
            if (btnHide == false)
            {
                HideSetting(1);
                btnHide = true;
            }
            else
            {
                HideSetting(2);
                btnHide = false;
            }
        }
        private void HideSetting(int mode)
        {
            try
            {
                if (mode == 1)
                {
                    tlpMain.ColumnStyles[0].Width = 0f;
                    tlpMain.ColumnStyles[1].Width = 100f;
                }
                else if (mode == 2)
                {
                    tlpMain.ColumnStyles[0].Width = 21.80f;
                    tlpMain.ColumnStyles[1].Width = 79.20f;
                }
            }
            catch { }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private async Task findarduino()       // 0.0 Connection
        {

            ManagementObjectSearcher objOSDetails =
               new ManagementObjectSearcher(
                   "SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'");
            ManagementObjectCollection osDetailsCollection = objOSDetails.Get();

            foreach (ManagementObject usblist in osDetailsCollection)
            {
                string GetData = usblist.ToString();
                //MessageBox.Show(GetData);
                if (usblist["Description"].ToString() == "Prolific USB-to-Serial Comm Port")     //
                {
                    string[] arrport =
                    usblist.GetPropertyValue("NAME").ToString().Split('(', ')');
                    string portX = arrport[1];
                    try
                    {
                        SerialPortConn.SerialPort.DtrEnable = true;
                        SerialPortConn.SerialPort.PortName = portX;
                        SerialPortConn.SerialPort.BaudRate = 115200;
                        SerialPortConn.SerialPort.Parity = Parity.None;
                        SerialPortConn.SerialPort.StopBits = StopBits.One;
                        SerialPortConn.SerialPort.DataBits = 8;

                        SerialPortConn.SerialPort.Handshake = Handshake.None;
                        SerialPortConn.SerialPort.RtsEnable = true;
                        SerialPortConn.SerialPort.Open();
                        SerialPortConn.SerialPort.DataReceived += serialPort1_DataReceived;
                        //lblcon.Text = "Connected";
                        timer1.Start();
                    }
                    catch
                    {
                        continue;
                    }
                }
                else if (usblist["Description"].ToString() == "ATEN USB to Serial Bridge")     //
                {
                    string[] arrport =
                    usblist.GetPropertyValue("NAME").ToString().Split('(', ')');
                    string portX = arrport[1];
                    try
                    {
                        SerialPortConn.SerialPort.DtrEnable = true;
                        SerialPortConn.SerialPort.PortName = portX;
                        SerialPortConn.SerialPort.BaudRate = 115200;
                        SerialPortConn.SerialPort.Parity = Parity.None;
                        SerialPortConn.SerialPort.StopBits = StopBits.One;
                        SerialPortConn.SerialPort.DataBits = 8;

                        SerialPortConn.SerialPort.Handshake = Handshake.None;
                        SerialPortConn.SerialPort.RtsEnable = true;
                        SerialPortConn.SerialPort.Open();
                        SerialPortConn.SerialPort.DataReceived += serialPort1_DataReceived;
                        //lblcon.Text = "Connected";
                        timer1.Start();
                    }
                    catch
                    {
                        continue;
                    }
                }

            }


        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)  // 0.1 Read Serial
        {

            try
            {
                string Read = SerialPortConn.SerialPort.ReadLine();

                setText(Read);
            }
            catch { }

        }
        private void setText(string val) // 0.2 Invock Reqeuired
        {

            if (this.lblMeasure.InvokeRequired)
            {
                serialCalback scb = new serialCalback(setText);
                this.Invoke(scb, new object[] { val });

            }
            else
            {
                lblMeasure.Text = val;
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
           await CheckMode();
        }
        bool cRegis = false;
        bool cDiode = false;
        bool cVoltage = false;
        private async Task CheckMode()
        {
            try
            {
                if (rdbRegis.Checked && cRegis == false)
                {
                    SerialPortConn.SerialPort.WriteLine("conf:res");
                    cRegis = true;
                    cDiode = false;
                    cVoltage = false;
                }
                else if (rdbDiode.Checked && cDiode == false)
                {
                    SerialPortConn.SerialPort.WriteLine("conf:diod");
                    cRegis = false;
                    cDiode = true;
                    cVoltage = false;
                }
                else if (rdbVoltage.Checked && cVoltage == false)
                {
                    SerialPortConn.SerialPort.WriteLine("conf:volt:dc");
                    cRegis = false;
                    cDiode = false;
                    cVoltage = true;
                }
            }
            catch { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;


namespace MeasurementAutoKey
{
    public static class SerialPortConn
    {
        private static SerialPort _serialport = new SerialPort();
        public static SerialPort SerialPort
        {
            get { return _serialport; }
            set { _serialport = value; }
        }
    }

}

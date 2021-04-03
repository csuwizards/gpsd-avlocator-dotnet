/* using System;
using System.IO.Ports;

class PortDataReceived
{
    static SerialPort _serialPort;
    public static void Main()
    {       
        Console.Write("Port name: ");
        string port = Console.ReadLine();
    //    Console.Write("baudrate: ");
       string baudrate = "9600";

       // Create a new SerialPort on indicated serial device (port)
       _serialPort = new SerialPort(port, int.Parse(baudrate));

       // Set the read/write timeouts
       _serialPort.ReadTimeout = 1500;
       _serialPort.WriteTimeout = 1500;
       //_serialPort.Open();


        //SerialPort mySerialPort = new SerialPort("COM1");

        // _serialPort.BaudRate = 9600;
        // _serialPort.Parity = Parity.None;
        // _serialPort.StopBits = StopBits.One;
        // _serialPort.DataBits = 8;
        // _serialPort.Handshake = Handshake.None;
        _serialPort.RtsEnable = true;

        _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

        _serialPort.Open();

        Console.WriteLine("Press any key to continue...");
        Console.WriteLine();
        Console.ReadKey();
        _serialPort.Close();
    }

    private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        //Console.WriteLine("Rx Data: ");
        Console.Write(indata);
    }
}
*/

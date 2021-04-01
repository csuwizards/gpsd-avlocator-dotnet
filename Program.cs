using System;
using System.IO.Ports;

namespace serial_read
{
    class Program
   {
     static SerialPort _serialPort;
     static void Main(string[] args)
     {
      bool notStop = true;

       Console.Write("Port no: ");
       string port = Console.ReadLine();
    //    Console.Write("baudrate: ");
       string baudrate = "9600";
   
       // Create a new SerialPort on indicated serial device (port)
       _serialPort = new SerialPort(port, int.Parse(baudrate));
   
       // Set the read/write timeouts
       _serialPort.ReadTimeout = 1500;
       _serialPort.WriteTimeout = 1500;
       _serialPort.Open();

       do {
         Read();
       } while (notStop);

       _serialPort.Close();
     }
   
     public static void Read()
     {
       try
       {
         string message = _serialPort.ReadLine();
         Console.WriteLine(message);
       }
       catch (TimeoutException) { }
     }
   }
}
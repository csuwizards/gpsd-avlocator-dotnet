using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace serial_read
{
    class Program
    {
     static SerialPort _serialPort;
     static string port = "/dev/ttyUSB0";
     static async Task Main(string[] args)
     {
      bool notDone = true;
	    var msgQueue = Channel.CreateUnbounded<string>();
      string baudrate = "9600";
   
       // Create a new SerialPort on indicated serial device (port)
       _serialPort = new SerialPort(port, int.Parse(baudrate));
   
       // Set the read/write timeouts
       _serialPort.ReadTimeout = 1500;
       _serialPort.WriteTimeout = 1500;
       _serialPort.Open();

      _ = Task.Factory.StartNew(async () => 
        {
          try {
            while (notDone) {
                  string bufr = _serialPort.ReadLine();
                  // Console.WriteLine($"forwarded: {bufr.Split(",")[0]}");
                  await msgQueue.Writer.WriteAsync(bufr);
            } 
          }
          catch {
            // file.Close();
          }
          msgQueue.Writer.Complete();
        });

      await foreach (var item in msgQueue.Reader.ReadAllAsync())
      {
        Console.WriteLine($"+ {item}");
        await Task.Delay(100);
      }
       _serialPort.Close();
     }
   }
}
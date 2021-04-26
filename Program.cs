﻿using System;
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
      

      //  Console.Write("Port no: ");
      //  string port = Console.ReadLine();
    //    Console.Write("baudrate: ");
       string baudrate = "9600";
   
       // Create a new SerialPort on indicated serial device (port)
       _serialPort = new SerialPort(port, int.Parse(baudrate));
   
       // Set the read/write timeouts
       _serialPort.ReadTimeout = 1500;
       _serialPort.WriteTimeout = 1500;
       _serialPort.Open();

      _ = Task.Factory.StartNew(async () => 
        {
          // for (int i=0; i<50; i++) {
          while (notDone) {
                string bufr = _serialPort.ReadLine();
                Console.WriteLine($"read & sent: {bufr}");
              await msgQueue.Writer.WriteAsync(bufr);
          } 
          msgQueue.Writer.Complete();
        });

await foreach (var item in msgQueue.Reader.ReadAllAsync())
{
  Console.WriteLine($"Received: {item}");
  // await Task.Delay(250);
}
       _serialPort.Close();
     }
   
    // public static Read() {
        // Console.WriteLine($"Thread={Thread.CurrentThread.ManagedThreadId} Write a sentence and see each word reversed: ");
        // var msg = Console.ReadLine();
        // Console.WriteLine("");
    // 
        // foreach (var s in msg.Split())
        // {
        	  // await messageReverser.QueueForProcessing(s, cancellationTokenSource.Token);
        // }
    //  }

    //  public static void oldRead()
    //  {
      //  try
      //  {
        //  string message = _serialPort.ReadLine();
        //  Console.WriteLine(message);
      //  }
      //  catch (TimeoutException) { }
    //  }
   }
}
using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using MsgProcessor;

namespace serial_read
{
    class Program
    {
     static SerialPort _serialPort;
     public static async Task Main(string[] args)
     {
      bool notStop = true;
	    var messageQueue = Channel.CreateUnbounded<string>();
	    var messageReverser = new StringReverser(messageQueue);

	    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
	    messageReverser.StartProcessing(cancellationTokenSource.Token);

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
   
    public static void Read() {
        Console.WriteLine($"Thread={Thread.CurrentThread.ManagedThreadId} Write a sentence and see each word reversed: ");
        var msg = Console.ReadLine();
        Console.WriteLine("");
    
        foreach (var s in msg.Split())
        {
        	  await messageReverser.QueueForProcessing(s, cancellationTokenSource.Token);
        }
     }

     public static void oldRead()
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
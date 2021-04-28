using System;
<<<<<<< HEAD
// using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
=======
using System.Text;
using System.IO;
using System.IO.Ports;
// using NMEAParser;
// using NMEAParser.Exceptions;
>>>>>>> refs/remotes/origin/master

namespace serial_read
{
    class Program
    {
     static SerialPort _serialPort;
<<<<<<< HEAD
     static string port = "/dev/ttyUSB0";
     static async Task Main(string[] args)
     {
      bool notDone = true;
	    var msgQueue = Channel.CreateUnbounded<string>();
      string baudrate = "9600";
=======
     static readonly NmeaParser _parser = new NmeaParser();
     static void Main(string[] args)
     {
      bool notStop = true;
        try {
            if (args.Length < 1) {
                System.Console.WriteLine("Please retry, & enter the name of a [string] serialport argument.");
                }
            else if (args[0].Contains("-f")) { 
                string[] s = string.Join(" ", args).Split("-f ");
                // Console.WriteLine($"total command string was: {s}");
                string l;
                var fs = new FileStream(s[1], FileMode.Open, FileAccess.Read);
                using (var sr = new StreamReader(fs, Encoding.UTF8)) { 
                  // Attempt to read & parse the line...
                  while ((l = sr.ReadLine()) != null) { 
                    try {
                          var m = _parser.Parse(l);
                      } catch (System.FormatException) {
                          Console.WriteLine($"<dropped {l}");
                      } catch (UnknownTypeException) {
                          Console.WriteLine($"<dropped unknown class {l.Split(",")[0]}");
                      }
                    }
                }
            } else {
                // Create a new SerialPort on indicated serial device (port)
                //    then Set the read/write timeouts

               string port = Console.ReadLine();
               string baudrate = "9600";
>>>>>>> refs/remotes/origin/master
   
               // Create a new SerialPort on indicated serial device (port)
               _serialPort = new SerialPort(port, int.Parse(baudrate));
   
               // Set the read/write timeouts
               _serialPort.ReadTimeout = 1500;
               _serialPort.WriteTimeout = 1500;
               _serialPort.Open();

<<<<<<< HEAD
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
=======
               while (notStop) {
                 Read();
               } 
            }
        _serialPort.Close();
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }

>>>>>>> refs/remotes/origin/master
     }
   }
 }
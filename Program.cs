using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace serial_read
{
    class Program
    {
        static SerialPort _serialPort;
        static string baudrate = "9600";
        static NmeaParser _parser = new NmeaParser();
        static async Task Main(string[] args)
        {
           string port = "/dev/ttyUSB0";
           bool notDone = true;
           var msgQueue = Channel.CreateUnbounded<string>();
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
                               Console.WriteLine(m);
                           } catch (System.FormatException) {
                               Console.WriteLine($"<dropped {l}>");
                           } catch (UnknownTypeException) {
                               Console.WriteLine($"<dropped unknown class {l.Split(",")[0]}>");
                           }
                        }
                   }
               } else {
                   // Create a new SerialPort on indicated serial device (port)
                   //    then Set the read/write timeouts
                   _serialPort = new SerialPort(port, int.Parse(baudrate));
   
                   // Set the read/write timeouts
                   _serialPort.ReadTimeout = 1500;
                   _serialPort.WriteTimeout = 1500;
                   _serialPort.Open();

                   _ = Task.Factory.StartNew(async () => {
                        while (notDone) {
                           string bufr = _serialPort.ReadLine();
                           try {
                              // Console.WriteLine($"forwarded: {bufr.Split(",")[0]}");
                                NmeaMessage m = _parser.Parse(bufr);
                                Console.WriteLine(m);
                                await msgQueue.Writer.WriteAsync(bufr);
                                //   msgQueue.Writer.Complete();
                            } catch (System.FormatException) {
                                   Console.WriteLine($"<dropped {bufr}>");
                            } catch (UnknownTypeException) {
                                   Console.WriteLine($"<dropped unknown class {bufr.Split(",")[0]}>");
                             // file.Close();
                           }
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
        _serialPort.Close();
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }

     }
   }
 }
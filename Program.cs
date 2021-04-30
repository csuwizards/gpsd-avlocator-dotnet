using System;
// using System.IO;
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
         static string port = "/dev/ttyUSB0";
         static async Task Main(string[] args)
         {
                var msgQueue = Channel.CreateUnbounded<string>();
                string baudrate = "9600";
                NmeaParser _parser = new NmeaParser();

                bool notStop = true;
                try
                {
                    if (args.Length < 1)
                    {
                        System.Console.WriteLine("Please retry, & enter the name of a [string] serialport argument.");
                    }
                    else if (args[0].Contains("-f"))
                    {
                        string[] s = string.Join(" ", args).Split("-f ");
                        // Console.WriteLine($"total command string was: {s}");
                        string l;
                        var fs = new FileStream(s[1], FileMode.Open, FileAccess.Read);
                        using (var sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            // Attempt to read & parse the line...
                            while ((l = sr.ReadLine()) != null)
                            {
                                try
                                {
                                    var m = _parser.Parse(l);
                                }
                                catch (System.FormatException)
                                {
                                    Console.WriteLine($"<dropped {l}");
                                }
                                catch (UnknownTypeException)
                                {
                                    Console.WriteLine($"<dropped unknown class {l.Split(",")[0]}");
                                }
                            }
                        }
                    }
                    else
                    {
                        // Create a new SerialPort on indicated serial device (port)
                        //    then Set the read/write timeouts

                        string port = Console.ReadLine();

                        // Create a new SerialPort on indicated serial device (port)
                        SerialPort _serialPort = new SerialPort(port, int.Parse(baudrate));

                        // Set the read/write timeouts
                        _serialPort.ReadTimeout = 1500;
                        _serialPort.WriteTimeout = 1500;
                        _serialPort.Open();

                        _ = Task.Factory.StartNew(async () => {
                            try
                            {
                                while (notStop) {
                                    string bufr = _serialPort.ReadLine();
                                    // Console.WriteLine($"forwarded: {bufr.Split(",")[0]}");
                                    await msgQueue.Writer.WriteAsync(bufr);
                                }
                            } catch {
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
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
              }
         }
}
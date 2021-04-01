# serial-read
Enable File Integrity Monitoring on directory trees
![](doc/csGPSdevReadOnPi.gif)

This is serial-read (Program.cs), wherein I perform a C#/.NET read of the indicated
port on the Raspberry Pi.

This is a prelude to a full-blown GPS system which will read GPS messages from a 
NEO-6 u-blox type device, streaming NMEA messages.

It is based on the .NET 5.0 serial example from the uSoft documentation.

The u-BLOX is connected to an AtMega-type serial-to-USB plug-in converter

The original inspiration was created/saved elsewhere in github.com/jpsthecelt, originally
as a python program.

3.30.21-jps

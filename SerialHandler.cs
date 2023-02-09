using System.Management;
using System.Runtime.InteropServices;
using System.IO.Ports;

namespace tcom {
    internal static class SerialHandler {        
        internal static void Display() {
            // display COM devices
            Console.WriteLine("Available serial devices:");

            int i = 1;
            foreach (var device in SerialPort.GetPortNames()) {
                Console.WriteLine($"{i} | {device}");
                i++;
            }

            Console.Write("Select a port number: ");

            int COM = -1;
            for (; ; ) {
                string? l = Console.ReadLine();

                if (l is null || l == "" || !int.TryParse(l, out COM))
                    Console.Write("Enter a valid number: ");
                else
                    break;
            }
        }
    }
}
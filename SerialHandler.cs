using System.IO.Ports;

namespace tcom {
    internal static class SerialHandler {
        internal static Thread ReadThread { get; private set; }
        internal static SerialPort Port { get; private set; }

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

            Start(COM);
        }

        public static void Start(int COM) {
            try {
                Port = new($"COM{COM}", 115200, Parity.None, 8, StopBits.One);
                Port.Open();
                Port.ReceivedBytesThreshold = 100;

                ReadThread = new(Read);
                ReadThread.Start();
            }
            catch (IOException ioex) {
                Console.WriteLine($"Error opening COM{COM}: {ioex.Message}");
                Environment.Exit(-1);
            }
            catch (ThreadInterruptedException) {
                Console.WriteLine("Thread interrupted. Killing...");
                
            } catch (Exception e) {
                Console.WriteLine($"Unexpected error opening port: {e.Message}");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Connected to COM{COM}.");
        }

        internal static void Read() { 
            while (true) {
                try {
                    string? line = Port.ReadLine();
                    Console.WriteLine(line);
                }
                catch (OperationCanceledException) {
                    Console.WriteLine("Device disconnected.");
                    Environment.Exit(-1);
                }
                catch (TimeoutException) {
                    Console.WriteLine("Readline timed out.");
                    Environment.Exit(-1);
                }
            }
        }
    }
}
using System.IO.Ports;

namespace tcom {
    internal static class SerialHandler {
        internal static Thread ReadThread { get; private set; }
        internal static SerialPort Port { get; private set; }

        private static int COM { get; set; }

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

            SerialHandler.COM = COM;
            Start();
        }

        public static void Start() {
            try {
                Port = new($"COM{COM}", 115200, Parity.None, 8, StopBits.One) {
                    RtsEnable = true,
                    DtrEnable = true,
                    Handshake = Handshake.None
                };
                Port.DataReceived += new SerialDataReceivedEventHandler(OnRecieveData);
                Port.Open();
            }
            catch (IOException ioex) {
                Console.WriteLine($"Error opening COM{COM}: {ioex.Message}");
                Environment.Exit(-1);
            } catch (Exception e) {
                Console.WriteLine($"Unexpected error opening port: {e.Message}");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Connected to COM{COM}.");
        }

        private static void OnRecieveData(object sender, SerialDataReceivedEventArgs e) {
            try {
                SerialPort sp = (SerialPort)sender;
                string payload = sp.ReadLine();
                Console.WriteLine(payload);
            }
            catch (Exception ex) {
                Console.WriteLine($"Unexpected error reading data: {ex.Message}");
                Environment.Exit(-1);
            }
        }
    }
}
using System.Diagnostics;
using System.Transactions;
using tcom;

namespace Program {
    internal static class Program {
        private enum CnxType {
            port,
            display,
            find
        }

        internal static void Main(string[] args) {
            // format
            // Usage: tcom [-Tr] <-d | -f | port>
            // options:
            // -T: tunnel             | tunnels port from windows into wsl
            // -t timestamp:          | adds timestamp to output
            // -r: regex highlighting | specify a highlighting schema
            // -d: display            | displays COM devices
            // -f: find               | attempts to find the COM device

            // setup program destructor
            var curProc = Process.GetCurrentProcess();
            curProc.Exited += OnApplicationExit;
            Console.CancelKeyPress += OnApplicationExit;

            bool tunnel = false,
                regex = false,
                timestamp = false;
            var cnxType = CnxType.port;
            int COM = -2;

            // parse args

            var printArgs = () => Console.WriteLine("Usage: tcom [-tr] <-d | -f | port>");

            foreach (var arg in args)
                if (arg[0] == '-')
                    switch (arg) {
                        case "-T":
                            tunnel = true;
                            break;
                        case "-t":
                            timestamp = true;
                            break;
                        case "-r":
                            regex = true;
                            break;
                        case "-d":
                            cnxType = CnxType.display;
                            COM = -1;
                            break;
                        case "-f":
                            cnxType = CnxType.find;
                            COM = -1;
                            break;
                    } else if (!int.TryParse(arg, out COM)) {
                        Console.WriteLine("Invalid port number.");
                        Environment.Exit(1);
                    }
            if (args.Length == 0 || (COM == -2 && cnxType == CnxType.port)) {
                printArgs();
                OnApplicationExit(null, null!);
            }

            // setup serial port
            if (cnxType == CnxType.port) {
                throw new NotImplementedException();
            } else if (cnxType == CnxType.display) {
                SerialHandler.Display();
            } else if (cnxType == CnxType.find) {
                throw new NotImplementedException();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            OnApplicationExit(null, null!);
        }

        internal static void OnApplicationExit(object? u, EventArgs e) {
            Console.WriteLine("Exiting...");
            if (SerialHandler.Port is not null && SerialHandler.Port.IsOpen) {
                SerialHandler.Port.Close();
                SerialHandler.Port.Dispose();
            }

            Environment.Exit(0);
        }
    }
}
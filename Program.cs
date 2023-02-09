using System.Diagnostics;
using System.Transactions;

namespace Program {
    internal class Program {
        private enum CnxType {
            port,
            display,
            find
        }

        internal static void Main(string[] args) {
            // format
            // Usage: tcom [-tr] <-d | -f | port>
            // options:
            // -t: tunnel             | tunnels port from windows into wsl
            // -r: regex highlighting | specify a highlighting schema
            // -d: display            | displays COM devices
            // -f: find               | attempts to find the COM device

            // setup program destructor
            var curProc = Process.GetCurrentProcess();
            curProc.Exited += OnApplicationExit;

            bool tunnel = false,
                regex = false;
            var cnxType = CnxType.port;
            int COM = -2;

            // parse args

            var printArgs = () => Console.WriteLine("Usage: tcom [-tr] <-d | -f | port>");

            foreach (var arg in args)
                if (arg[0] != '-')
                    switch (arg) {
                        case "-t":
                            tunnel = true;
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

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            OnApplicationExit(null, null!);
        }

        internal static void OnApplicationExit(object? u, EventArgs e) {
            Environment.Exit(0);
        }
    }
}
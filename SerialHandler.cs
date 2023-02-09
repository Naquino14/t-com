using System.Management;
using System.Runtime.InteropServices;

namespace tcom {
    internal static class SerialHandler {
        internal static void Display() {
            // display COM devices
            var ports = GetCOMDevices();
            Console.WriteLine("COM devices:");
            foreach (var port in ports)
                Console.WriteLine(port);
        }

        internal static List<DeviceInfo> GetCOMDevices() {
            List<DeviceInfo> devices = new();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                using var search = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBHub");
                using var collection = search.Get();

                foreach (var device in collection) {
                    devices.Add(new DeviceInfo(
                        (string)device.GetPropertyValue("DeviceID"),
                        (string)device.GetPropertyValue("PNPDeviceID"),
                        (string)device.GetPropertyValue("Description")
                    ));
                }
            }

            return devices;
        }
    }

    internal class DeviceInfo {
        internal DeviceInfo(string deviceID, string pnpDeviceID, string description) {
            DeviceID = deviceID;
            PnpDeviceID = pnpDeviceID;
            Description = description;
        }

        internal string DeviceID { get; private set; }
        internal string PnpDeviceID { get; private set; }
        internal string Description { get; private set; }
    }
}
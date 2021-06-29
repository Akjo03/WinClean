using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WinClean {
    public class WinHelper {

        private RegistryHelper registryRef;

        public WinHelper(RegistryHelper registryRef) {
            this.registryRef = registryRef;
        }

        public bool IsWindows() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public bool IsWindows10() {
            return registryRef.ReadAny(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName").StartsWith("Windows 10");
        }
    }
}

using System.Runtime.InteropServices;

namespace WinClean {
    public class SystemHelper {
        public static bool IsWindows() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}

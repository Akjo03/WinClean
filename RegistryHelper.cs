using WinClean.resources;

using Microsoft.Win32;

namespace WinClean {
    public class RegistryHelper {
        public void Write(string name, string value) {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinClean")) {
                key.SetValue(name, value);
            }
        }

        public string Read(string name) {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinClean")) {
                if (key != null) {
                    return key.GetValue(name).ToString();
                } else {
                    return null;
                }
            }
        }

        public void Delete(string name) {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinClean")) {
                if (key != null) {
                    key.DeleteValue(name);
                }
            }
        }
    }
}

using Microsoft.Win32;

namespace WinClean {
    public class RegistryHelper {
        /// <summary>
        /// Writes to the WinClean registry (at Computer\HKEY_CURRENT_USER\SOFTWARE\WinClean)
        /// </summary>
        /// <param name="name">The name of the value. If the value doesn't exist yet, it will be created.</param>
        /// <param name="value">The value itself.</param>
        public void Write(string name, string value) {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinClean"); key.SetValue(name, value);
        }

        /// <summary>
        /// Reads from the WinClean registry (at Computer\HKEY_CURRENT_USER\SOFTWARE\WinClean)
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <returns>The value</returns>
        public string Read(string name) {using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinClean");
            if (key != null) {
                return key.GetValue(name).ToString();
            } else {
                return null;
            }
        }

        /// <summary>
        /// Reads any value in the registry
        /// </summary>
        /// <param name="registryMain">The main sub key. Must be from the Microsoft.Win32.Registry class.</param>
        /// <param name="subkey">The subkey where the value is located at.</param>
        /// <param name="name">The name of the value.</param>
        /// <returns>The value</returns>
        public string ReadAny(RegistryKey registryMain, string subkey, string name) {
            using RegistryKey key = registryMain.OpenSubKey(subkey); if (key != null) {
                return key.GetValue(name).ToString();
            } else {
                return null;
            }
        }

        /// <summary>
        /// Checks if a value in the WinClean registry exists. (at Computer\HKEY_CURRENT_USER\SOFTWARE\WinClean)
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <returns>If the value exists.</returns>
        public bool ValueExists(string name) {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinClean"); return key.GetValue(name) != null;
        }

        /// <summary>
        /// Deletes a value in the WinClean registry. (at Computer\HKEY_CURRENT_USER\SOFTWARE\WinClean)
        /// </summary>
        /// <param name="name">The name of the value.</param>
        public void Delete(string name) {using RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinClean");
            if (key != null) {
                key.DeleteValue(name);
            }
        }
    }
}

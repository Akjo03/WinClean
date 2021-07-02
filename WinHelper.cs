using WinClean.resources;

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WinClean {

    using SLID = Guid;

    public class WinHelper {

        private ConsoleHelper consoleRef;
        private RegistryHelper registryRef;

        public WinHelper(ConsoleHelper consoleRef, RegistryHelper registryRef) {
            this.consoleRef = consoleRef;
            this.registryRef = registryRef;
        }

        public bool IsWindows() {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public bool IsWindows10() {
            return registryRef.ReadAny(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName").StartsWith("Windows 10");
        }

        public enum SL_GENUINE_STATE {
            SL_GEN_STATE_IS_GENUINE = 0,
            SL_GEN_STATE_INVALID_LICENSE = 1,
            SL_GEN_STATE_TAMPERED = 2,
            SL_GEN_STATE_OFFLINE = 3,
            SL_GEN_STATE_LAST = 4
        }

        [DllImport("Slwga.dll", EntryPoint = "SLIsGenuineLocal", CharSet = CharSet.None, ExactSpelling = false, SetLastError = false, PreserveSig = true, CallingConvention = CallingConvention.Winapi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        [PreserveSig()]
        internal static extern uint SLIsGenuineLocal(ref SLID slid, [In, Out] ref SL_GENUINE_STATE genuineState, IntPtr val3);

        public bool IsActivated() {
            bool _IsGenuineWindows;
            Guid ApplicationID = new Guid("55c92734-d682-4d71-983e-d6ec3f16059f");
            SLID windowsSlid = (Guid)ApplicationID;
            
            try {
                SL_GENUINE_STATE genuineState = SL_GENUINE_STATE.SL_GEN_STATE_LAST;
                uint ResultInt = SLIsGenuineLocal(ref windowsSlid, ref genuineState, IntPtr.Zero);
                if (ResultInt == 0) {
                    _IsGenuineWindows = (genuineState == SL_GENUINE_STATE.SL_GEN_STATE_IS_GENUINE);
                } else {
                    consoleRef.WriteError(Strings.Part1_ActivationCheck_Error);
                    return false;
                }
            } catch (Exception) {
                consoleRef.WriteError(Strings.Part1_ActivationCheck_Error);
                return false;
            }
            return _IsGenuineWindows;
        }
    }
}

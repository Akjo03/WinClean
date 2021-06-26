using WinClean.resources;

using System;
using System.Runtime.InteropServices;

namespace WinClean {
    public class ConsoleHelper {
        // === Console Output Methods ===
        public static void ConsoleWrite(string text) {
            Console.WriteLine("[WinClean] " + text);
        }

        public static void ConsoleWrite(string name, string text) {
            Console.WriteLine("[" + name + "] " + text);
        }

        public static void ConsoleWrite(string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine("[WinClean] " + text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleWrite(string name, string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine("[" + name + "] " + text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleWriteError(string text) {
            ConsoleWrite("ERROR", text, ConsoleColor.Red);
        }

        public static void ConsoleWriteWarn(string text) {
            ConsoleWrite("WARN", text, ConsoleColor.DarkYellow);
        }

        public static void ConsoleFatal(int exitCode, string reason) {
            ConsoleWrite("FATAL", Strings.FatalErrorOccured + " | Reason: " + reason);
            ConsoleExit(exitCode, true);
        }

        public static void ConsoleExit(int exitCode, bool message) {
            if (message) {
                ConsoleWrite(Strings.ExitingWithExitCode.Replace("{exitCode}", exitCode.ToString()));
            }
            Environment.Exit(exitCode);
        }

        public static void ConsoleTitle(string title) {
            Console.Title = "WinClean " + WinClean.Version + " | " + title;
        }

        public static void ConsoleClear() {
            Console.Clear();
        }

        // === Console Input Methods ===


        // === Console Font ===
        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0) {
            ConsoleWrite("Set Current Font: " + font);

            FontInfo before = new FontInfo {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before)) {

                FontInfo set = new FontInfo {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set)) {
                    ConsoleWriteError(new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).Message);
                }

                FontInfo after = new FontInfo {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            } else {
                ConsoleWriteError(new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).Message);
                return null;
            }
        }
    }
}
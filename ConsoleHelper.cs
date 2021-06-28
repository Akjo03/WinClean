using WinClean.resources;

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace WinClean {
    public class ConsoleHelper {
        // === Console Output ===

        public void Write(string text) {
            Console.WriteLine(" [WinClean] " + text);
        }

        public void Write(string name, string text) {
            if (name != null) {
                if (name.Length <= 0) {
                    Console.WriteLine(" " + text);
                }
                else {
                    Console.WriteLine(" [" + name + "] " + text);
                }
            }
            else {
                Write(text);
            }
        }

        public void Write(string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Write(string name, string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Write(name, text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteError(string text) {
            Write("ERROR", text, ConsoleColor.Red);
        }

        public void WriteWarn(string text) {
            Write("WARN", text, ConsoleColor.DarkYellow);
        }

        public void FatalExit(int exitCode, string reason) {
            Write("FATAL", Strings.FatalErrorOccured + " | Reason: " + reason);
            Exit(exitCode, true);
        }

        public void Title(string title) {
            Console.Title = "WinClean " + WinClean.Version + " | " + title;
        }

        public void Clear() {
            Console.Clear();
        }

        public void Exit(int exitCode, bool message) {
            if (message) {
                Write(Strings.ExitingWithExitCode.Replace("{exitCode}", exitCode.ToString()));
            }
            Environment.Exit(exitCode);
        }

        // === Console Input ===

        public struct SelectionOption {
            public int number { get; }
            public string optionText { get; }

            public SelectionOption(int number, string optionText) {
                this.number = number;
                this.optionText = optionText;
            }
        }

        public SelectionOption CreateSelection(string question, List<SelectionOption> options) {
            Write("", "");
            Write(question);
            foreach (SelectionOption option in options) {
                Write("", "           " + option.number + " - " + option.optionText);
            }
            Console.Write(" [" + Strings.Select + " (" + options.Min(option => option.number) + "-" + options.Max(option => option.number) + ")] > "); string input = Console.ReadLine();
            while (true) {
                if (String.IsNullOrWhiteSpace(input)) {
                    WriteError(Strings.SelectionEmpty);
                } else {
                    if (!int.TryParse(input, out int selectionNum)) {
                        WriteError(Strings.SelectionNotNumerical);
                    } else {
                        if (selectionNum >= options.Min(option => option.number) && selectionNum <= options.Max(option => option.number)) {
                            Write("", "");
                            return options.Find(option => option.number == selectionNum);
                        } else {
                            WriteError(Strings.SelectionNotInRange);
                        }
                    }
                }
                Console.Write(" [" + Strings.Select + " (" + options.Min(option => option.number) + "-" + options.Max(option => option.number) + ")] > "); input = Console.ReadLine();
            }
        }

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

        public FontInfo[] Font(string font, short fontSize = 0) {
            Write("Set Current Font: " + font);

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
                    WriteError(new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).Message);
                }

                FontInfo after = new FontInfo {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            } else {
                WriteError(new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).Message);
                return null;
            }
        }

        // === Console Window Methods ===
        const int SWP_NOSIZE = 0x0001;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static IntPtr ConsoleWindow { get; } = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public void WindowPosition(int x, int y) {
            SetWindowPos(ConsoleWindow, 0, x, y, 0, 0, SWP_NOSIZE);
        }
    }
}
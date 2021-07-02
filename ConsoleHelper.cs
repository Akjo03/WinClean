using WinClean.resources;

using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace WinClean {
    /// <summary>
    /// The ConsoleHelper class is used for many things. Mainly for reading and writing to console but also to run commands on a command line intepreter (like cmd.exe). Also has some other functions. 
    /// Use this class instead of the System.Console class as much as possible.
    /// </summary>
    public class ConsoleHelper {
        // === Console Output ===

        /// <summary>
        /// Writes to the Console with the name "WinClean"
        /// </summary>
        /// <param name="text">The text to write</param>
        public void Write(string text) {
            Console.WriteLine(" [WinClean] " + text);
        }

        /// <summary>
        /// Writes to the Console with the specified name
        /// </summary>
        /// <param name="name">The name to write as</param>
        /// <param name="text">The text to write</param>
        public void Write(string name, string text) {
            if (name != null) {
                if (name.Length <= 0) {
                    Console.WriteLine(" " + text);
                } else {
                    Console.WriteLine(" [" + name + "] " + text);
                }
            } else {
                Write(text);
            }
        }

        /// <summary>
        /// Writes to the Console with the name "WinClean" in the specified color
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="color">The color to write in</param>
        public void Write(string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Writes to the Console with the specified name and in the specified color
        /// </summary>
        /// <param name="name">The name to write as</param>
        /// <param name="text">The text to write</param>
        /// <param name="color">The color to write in</param>
        public void Write(string name, string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Write(name, text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Writes to the Console in the color red with the name "ERROR" 
        /// </summary>
        /// <param name="text">The text to write</param>
        public void WriteError(string text) {
            Write("ERROR", text, ConsoleColor.Red);
        }

        /// <summary>
        /// Writes to the Console in ogre with the name "WARN"
        /// </summary>
        /// <param name="text">The text to write</param>
        public void WriteWarn(string text) {
            Write("WARN", text, ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Exits WinClean with a FATAL error message with the specified reason.
        /// </summary>
        /// <param name="exitCode">The exit code</param>
        /// <param name="reason">The reason for the fatal error</param>
        public void FatalExit(int exitCode, string reason) {
            Write("FATAL", Strings.FatalErrorOccured + " | Reason: " + reason, ConsoleColor.Red);
            Exit(exitCode, true);
        }

        /// <summary>
        /// Changes the title of the Window
        /// </summary>
        /// <example>WinClean v0.0.0 | Example Title</example>
        /// <param name="title"></param>
        public void Title(string title) {
            Console.Title = "WinClean " + WinClean.Version + " | " + title;
        }

        /// <summary>
        /// Clears the console
        /// </summary>
        public void Clear() {
            Console.Clear();
        }

        /// <summary>
        /// Exits WinClean with the specified exit code.
        /// </summary>
        /// <param name="exitCode">The exit code</param>
        /// <param name="message">If a message should be displayed</param>
        /// <param name="clearOnExit">If the console should be cleared on exit</param>
        public void Exit(int exitCode, bool message = false, bool clearOnExit = true) {
            if (message) {
                Write(Strings.ExitingWithExitCode.Replace("{exitCode}", exitCode.ToString()));
                Thread.Sleep(GetReadingTime(Strings.ExitingWithExitCode.Replace("{exitCode}", exitCode.ToString())));
            }
            if (clearOnExit) {
                Console.Clear();
            }
            Environment.Exit(exitCode);
        }

        // === Console Input ===

        /// <summary>
        /// An option for a selection
        /// </summary>
        public struct SelectionOption {
            /// <summary>
            /// The associated number with the option
            /// </summary>
            public int Number { get; }

            /// <summary>
            /// The associated text with the option
            /// </summary>
            public string OptionText { get; }

            public SelectionOption(int number, string optionText) {
                Number = number;
                OptionText = optionText;
            }
        }

        /// <summary>
        /// Waits for enter with a default message
        /// </summary>
        public void EnterToContinue() {
            EnterToContinue(Strings.EnterToContinue);
        }

        /// <summary>
        /// Waits for enter with a specified message
        /// </summary>
        /// <param name="message">The message to write</param>
        public void EnterToContinue(string message) {
            Write(message);
            ConsoleKey key = Console.ReadKey().Key;
            while (key != ConsoleKey.Enter) {
                key = Console.ReadKey().Key;
            }
        }

        public ConsoleKey ReadKeys(string text, List<ConsoleKey> keys) {
            Console.Write(text);
            while (true) {
                ConsoleKey keyInput = Console.ReadKey(false).Key;
                foreach (ConsoleKey key in keys) {
                    if (keyInput == key) {
                        return keyInput;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the reading time for a specified text
        /// </summary>
        /// <param name="text">The text to get the reading time from</param>
        /// <returns>The reading time in milliseconds</returns>
        public int GetReadingTime(string text) {
            string[] words = text.Split(" ");
            int wordCount = 0;
            foreach (string word in words) {
                if (word.Length > 12) {
                    wordCount++;
                }
                wordCount++;
            }
            if (wordCount < 3) {
                return Convert.ToInt32(((((float) wordCount / 200 * 60) * 2) + 1) * 1000);
            } else {
                return Convert.ToInt32((float)wordCount / 200 * 60 * 1000);
            }
        }

        /// <summary>
        /// Gets the reading time for multiple texts
        /// </summary>
        /// <param name="texts">The list ôf texts to get the reading time from</param>
        /// <returns>The reading time in milliseconds</returns>
        public int GetReadingTime(List<string> texts) {
            int readingTime = 0;
            foreach (string text in texts) {
                readingTime += GetReadingTime(text);
            }
            return readingTime;
        }

        /// <summary>
        /// Creates a new question for the user with the specified question and the name of the input
        /// </summary>
        /// <param name="question">The question the user should answer</param>
        /// <param name="name">The name of the input</param>
        /// <returns>The input from the user</returns>
        public string CreateQuestion(string question, string name) {
            Write(question);
            Console.Write(" [" + name + "] > "); return Console.ReadLine().Trim();
        }

        /// <summary>
        /// Cretes a new selection for the user with the specified question and options
        /// </summary>
        /// <param name="question">The question the user should answer</param>
        /// <param name="options">The options the user has</param>
        /// <returns>The selected option</returns>
        public SelectionOption CreateSelection(string question, List<SelectionOption> options) {
            Write(question);
            foreach (SelectionOption option in options) {
                Write("", "           " + option.Number + " - " + option.OptionText);
            }
            Console.Write(" " + Strings.Select + " (" + options.Min(option => option.Number) + "-" + options.Max(option => option.Number) + ") > "); string input = Console.ReadLine();
            while (true) {
                if (String.IsNullOrWhiteSpace(input)) {
                    WriteError(Strings.SelectionEmpty);
                } else {
                    if (!int.TryParse(input, out int selectionNum)) {
                        WriteError(Strings.SelectionNotNumerical);
                    } else {
                        if (selectionNum >= options.Min(option => option.Number) && selectionNum <= options.Max(option => option.Number)) {
                            Write("", "");
                            return options.Find(option => option.Number == selectionNum);
                        } else {
                            WriteError(Strings.SelectionNotInRange);
                        }
                    }
                }
                Console.Write(" " + Strings.Select + " (" + options.Min(option => option.Number) + "-" + options.Max(option => option.Number) + ") > "); input = Console.ReadLine();
            }
        }

        // === Console Run ===

        /// <summary>
        /// Runs a command on cmd.exe (without showing any window)
        /// </summary>
        /// <param name="command">The command to run</param>
        /// <param name="admin">If the command should be runned as an admin</param>
        /// <param name="showOut">If the output of the command should be shown</param>
        /// <returns>The exit code of the command line</returns>
        public int RunCommand(string command, bool admin = false, bool showOut = false) {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            if (admin) {
                cmd.StartInfo.Verb = "runas";
            }
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            if (!cmd.WaitForExit(10000)) {
                cmd.Kill();
            }
            if (showOut) {
                Write("Console", cmd.StandardOutput.ReadToEnd());
            }
            return cmd.ExitCode;
        }

        /// <summary>
        /// Runs a command on cmd.exe (without showing any window) and searches the output for strings to check.
        /// </summary>
        /// <param name="command">The command to run</param>
        /// <param name="checks">A list of strings to check the output with</param>
        /// <param name="admin">If the command should be runned as an admin</param>
        /// <param name="showOut">If the output of the command should be shown</param>
        /// <returns>
        /// The exit code of the command line and a list of all the checks that have been found in the output.
        /// </returns>
        public (int, List<string>) RunCommandWithChecks(string command, List<string> checks, bool admin = false, bool showOut = false) {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            if (admin) {
                cmd.StartInfo.Verb = "runas";
            }
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            if (!cmd.WaitForExit(10000)) {
                cmd.Kill();
            }

            string output = cmd.StandardOutput.ReadToEnd();

            if (showOut) {
                Write("Console", output);
            }

            List<string> hits = new List<string>();

            foreach (string check in checks) {
                if (output.Contains(check)) {
                    hits.Add(check);
                }
            }

            return (cmd.ExitCode, hits);
        }

        /// <summary>
        /// Runs a command on powershell.exe (without showing any window)
        /// </summary>
        /// <param name="command">The command to run</param>
        /// <param name="admin">If the command should be runned as an admin</param>
        /// <param name="showOut">If the output of the command should be shown</param>
        /// <returns>The exit code of the command line</returns>
        public int RunCommandInPowerShell(string command, bool admin = false, bool showOut = false) {
            Process ps = new Process();
            ps.StartInfo.FileName = "powershell.exe";
            ps.StartInfo.RedirectStandardInput = true;
            ps.StartInfo.RedirectStandardOutput = true;
            ps.StartInfo.CreateNoWindow = true;
            ps.StartInfo.UseShellExecute = false;
            if (admin) {
                ps.StartInfo.Verb = "runas";
            }
            ps.Start();

            ps.StandardInput.WriteLine(command);
            ps.StandardInput.Flush();
            ps.StandardInput.Close();
            if (!ps.WaitForExit(10000)) {
                ps.Kill();
            }
            if (showOut) {
                Write("Console", ps.StandardOutput.ReadToEnd());
            }
            return ps.ExitCode;
        }

        /// <summary>
        /// Runs a command on powershell.exe (without showing any window) and searches the output for strings to check.
        /// </summary>
        /// <param name="command">The command to run</param>
        /// <param name="checks">A list of strings to check the output with</param>
        /// <param name="admin">If the command should be runned as an admin</param>
        /// <param name="showOut">If the output of the command should be shown</param>
        /// <returns>
        /// The exit code of the command line and a list of all the checks that have been found in the output.
        /// </returns>
        public (int, List<string>) RunCommandInPowerShellWithChecks(string command, List<string> checks, bool admin = false, bool showOut = false) {
            Process ps = new Process();
            ps.StartInfo.FileName = "powershell.exe";
            ps.StartInfo.RedirectStandardInput = true;
            ps.StartInfo.RedirectStandardOutput = true;
            ps.StartInfo.CreateNoWindow = true;
            ps.StartInfo.UseShellExecute = false;
            if (admin) {
                ps.StartInfo.Verb = "runas";
            }
            ps.Start();

            ps.StandardInput.WriteLine(command);
            ps.StandardInput.Flush();
            ps.StandardInput.Close();
            if (!ps.WaitForExit(10000)) {
                ps.Kill();
            }

            string output = ps.StandardOutput.ReadToEnd();

            if (showOut) {
                Write("Console", output);
            }

            List<string> hits = new List<string>();

            foreach (string check in checks) {
                if (output.Contains(check)) {
                    hits.Add(check);
                }
            }

            return (ps.ExitCode, hits);
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

        /// <summary>
        /// Changes the font of the console
        /// </summary>
        /// <param name="font">The font to change to</param>
        /// <param name="fontSize">The size of the font. If 0 the previous font size will stay</param>
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
    }
}
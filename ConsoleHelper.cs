using WinClean.resources;

using System;
using System.Globalization;
using System.Threading;

namespace WinClean {
    public class ConsoleHelper {
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
            ConsoleExit(-1, true);
        }

        public static void ConsoleExit(int exitCode, bool message) {
            if (message) {
                ConsoleWrite(Strings.ExitingWithExitCode + " " + exitCode + ".");
            }
            Environment.Exit(exitCode);
        }
    }
}
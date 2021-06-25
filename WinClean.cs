using WinClean.resources;

using System;

using static WinClean.ConsoleHelper;
using static WinClean.LocaleHelper;

namespace WinClean {
    /// <summary>
    /// Main class for WinClean
    /// 
    /// Author: Akjo03
    /// For Version: v0.0.0
    /// </summary>
    public class WinClean {

        public static string Version = "v0.0.0";

        /// <summary>
        /// Main entry point for WinClean
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args) {
            SetLang("en-us");
            ConsoleClear();
            ConsoleTitle("Welcome");
            ConsoleWrite(Strings.WelcomeMessage);
            ConsoleWrite(Strings.EarlyDevelopmentMessage);
            Console.ReadLine();
        }
    }
}
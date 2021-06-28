using WinClean.resources;

using System;
using System.Threading;

namespace WinClean {
    /// <summary>
    /// Main class for WinClean
    /// 
    /// Author: Akjo03
    /// For Version: v0.0.0
    /// </summary>
    public class WinClean {
        public static string Version { get; } = "v0.0.0";
        private ConsoleHelper Console { get; }
        private LocaleHelper Locale { get; }

        private WinClean(string[] args) {
            Console = new ConsoleHelper();
            Locale = new LocaleHelper(Console);
            Start();
        }

        /// <summary>
        /// Main entry point for WinClean
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args) {
            new WinClean(args);
        }

        public void Start() {
            Console.Font("Consolas", 24);
            Locale.SetLang("en-us");
            Console.Clear();
            Console.Title(Strings.WelcomeTitle);
            Console.Write(Strings.WelcomeMessage);
            Console.Write(Strings.EarlyDevelopmentMessage);
            Thread.Sleep(5000);
            Console.Exit(0, false);
        }
    }
}
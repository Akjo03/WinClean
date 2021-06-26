using WinClean.resources;

using System;
using System.Threading;

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

        public static string Version { get; } = "v0.0.0";

        private ConsoleHelper console;
        private LocaleHelper locale;

        private WinClean() {
            console = new ConsoleHelper();
            locale = new LocaleHelper(console);
            Start();
        }

        /// <summary>
        /// Main entry point for WinClean
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args) {
            new WinClean();
        }

        public void Start() {
            console.Font("Consolas", 24);
            locale.SetLang("en-us");
            console.Clear();
            console.Title(Strings.WelcomeTitle);
            console.Write(Strings.WelcomeMessage);
            console.Write(Strings.EarlyDevelopmentMessage);
            console.Write("", "");
            console.Write(Strings.EnterToExit);
            Console.ReadLine();
            console.Exit(0, false);
        }
    }
}
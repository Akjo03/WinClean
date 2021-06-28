using WinClean.resources;

using System.Threading;
using System.Collections.Generic;

namespace WinClean {
    /// <summary>
    /// Main class for WinClean
    /// 
    /// Author: Akjo03
    /// For Version: v0.0.0
    /// </summary>
    public class WinClean {
        public static string Version { get; } = "v0.0.0";

        public static List<int> availableParts = new List<int>() { 0 };
        public static List<string> availableLocale = new List<string>() { "en-us", "de-de" };

        private ConsoleHelper Console;

        private LocaleHelper Locale;

        private RegistryHelper Registry;

        private WinClean(string[] args) {
            // Create the console helper
            Console = new ConsoleHelper();

            // Create the locale helper and set the font and default language
            Console.Font("Consolas", 24);
            Locale = new LocaleHelper(Console);
            Locale.SetLang("en-us");
            Console.Clear();

            // Get command line arguments
            ArgumentParser argumentParser = new ArgumentParser(Console, Locale);
            (List<int> parts, string locale) = argumentParser.Parse(args);

            // Create the registry helper
            Registry = new RegistryHelper();

            // Start the WinClean program
            Start(parts, locale);
        }

        /// <summary>
        /// Main entry point for WinClean
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args) {
            new WinClean(args);
        }

        public void Start(List<int> parts, string locale) {
            Console.Clear();

            if (parts.Contains(0) && locale == null) {
                Part0_SelectLanguage();
            }

            Console.Clear();
            Console.Title(Strings.WelcomeTitle);
            Console.Write(Strings.WelcomeMessage);
            Console.Write(Strings.EarlyDevelopmentMessage);
            Thread.Sleep(Console.GetReadingTime(new List<string>() { Strings.WelcomeMessage, Strings.EarlyDevelopmentMessage }));
            Console.Exit(0, false);
        }

        private void Part0_SelectLanguage() {
            Console.Clear();
            var languageSelection = Console.CreateSelection(Strings.Selection_Language, new List<ConsoleHelper.SelectionOption>() {
                new ConsoleHelper.SelectionOption(1, Strings.Selection_Language_AnsEnglish),
                new ConsoleHelper.SelectionOption(2, Strings.Selection_Language_AnsGerman)
            });
            switch (languageSelection.number) {
                case 1:
                    Locale.SetLang("en-us");
                    break;

                case 2:
                    Locale.SetLang("de-de");
                    break;

                default:
                    Locale.SetLang("en-us");
                    break;
            }
            Console.Clear();
        }
    }
}
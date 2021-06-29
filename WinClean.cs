﻿using WinClean.resources;

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

        private WinHelper Windows;

        private WinClean(string[] args) {
            // Create the console helper
            Console = new ConsoleHelper();

            // Create the registry helper
            Registry = new RegistryHelper();

            // Create the windows (system) helper
            Windows = new WinHelper(Registry);

            // Create the locale helper and set the font
            Console.Font("Consolas", 24);
            Locale = new LocaleHelper(Console);
            if (Registry.ValueExists("locale")) {
                Locale.SetLocale(Registry.Read("locale"));
            } else {
                if (availableLocale.Contains(Thread.CurrentThread.CurrentUICulture.Name.ToLower())) {
                    Locale.SetLocale(Thread.CurrentThread.CurrentUICulture.Name.ToLower());
                } else {
                    Locale.SetLocale("en-us");
                }
            }
            Console.Clear();

            //Check if we are on windows 10
            if (!Windows.IsWindows()) {
                Console.WriteError(Strings.NotWindows);
                Console.EnterToContinue(Strings.EnterToExit);
                Console.Exit(-1, false);
            } else {
                if (!Windows.IsWindows10()) {
                    Console.WriteError(Strings.NotWindows10);
                    Console.EnterToContinue(Strings.EnterToExit);
                    Console.Exit(-1, false);
                }
            }

            // Get and parse command line arguments
            ArgumentParser argumentParser = new ArgumentParser(Console, Locale);
            (List<int> parts, string locale) = argumentParser.Parse(args);

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

            // Get language from user if part 0 is activated and not set already
            if (parts.Contains(0) && locale == null) {
                Part0_SelectLanguage();
            } else if (locale != null) {
                Locale.SetLocale(locale);
            }
            // Save the language to the registry
            if (Locale.GetLocale() != null) {
                Registry.Write("locale", Locale.GetLocale());
            }

            // Show Welcome message
            Console.Clear();
            Console.Title(Strings.WelcomeTitle);
            Console.Write(Strings.WelcomeMessage);
            Console.Write(Strings.EarlyDevelopmentMessage);
            Thread.Sleep(Console.GetReadingTime(new List<string>() { Strings.WelcomeMessage, Strings.EarlyDevelopmentMessage }));
            Console.Exit(0, false);
        }

        private void Part0_SelectLanguage() {
            Console.Clear();
            var currentLanguageConfirmation = Console.CreateSelection(Strings.Selection_Language_CurrentLanguage, new List<ConsoleHelper.SelectionOption>() {
                new ConsoleHelper.SelectionOption(1, Strings.Selection_Language_CurrentLanguage_Correct),
                new ConsoleHelper.SelectionOption(2, Strings.Selection_Language_CurrentLanguage_Wrong)
            });
            if (currentLanguageConfirmation.Number == 1) {
                return;
            }
            var languageSelection = Console.CreateSelection(Strings.Selection_Language, new List<ConsoleHelper.SelectionOption>() {
                new ConsoleHelper.SelectionOption(1, Strings.Selection_Language_AnsEnglish),
                new ConsoleHelper.SelectionOption(2, Strings.Selection_Language_AnsGerman)
            });
            switch (languageSelection.Number) {
                case 1:
                    Locale.SetLocale("en-us");
                    break;

                case 2:
                    Locale.SetLocale("de-de");
                    break;

                default:
                    Locale.SetLocale("en-us");
                    break;
            }
            Console.Clear();
        }
    }
}
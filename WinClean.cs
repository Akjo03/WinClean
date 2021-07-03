using WinClean.resources;

using System.Threading;
using System.Collections.Generic;

namespace WinClean {
    /// <summary>
    /// Main class for WinClean
    /// 
    /// Author: Akjo03
    /// For Version: v0.0.1
    /// </summary>
    public class WinClean {
        /// <summary>
        /// Version of WinClean
        /// </summary>
        public static string Version { get; } = "v0.0.1";

        /// <summary>
        /// List of parts that are availble in this version of WinClean
        /// </summary>
        public static readonly  List<int> availableParts = new List<int>() { 0, 1 };

        /// <summary>
        /// List of available locales (languages) in this version of WinClean
        /// </summary>
        public static readonly  List<string> availableLocale = new List<string>() { "en-us", "de-de" };

        private readonly ConsoleHelper Console;

        private readonly LocaleHelper Locale;

        private readonly RegistryHelper Registry;

        private readonly WinHelper Windows;

        /// <summary>
        /// Creates the main instance of WinClean. Should only be called once.
        /// This will instantiate all the helpers and set the default font, locale and parses all the command line arguments.
        /// This will also check if WinClean is being ran on Windows 10.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        private WinClean(string[] args) {
            // Create the console helper
            Console = new ConsoleHelper();

            // Create the registry helper
            Registry = new RegistryHelper();

            // Create the windows (system) helper
            Windows = new WinHelper(Console, Registry);

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
            var mainApplication = new WinClean(args);
        }

        /// <summary>
        /// Starts WinClean with the parts that it should run and a locale.
        /// </summary>
        /// <param name="parts">A list of parts that should be run</param>
        /// <param name="locale">The locale that the WinClean app should run on. If this is null the user can choose the locale as long as Part 0 is ran.</param>
        private void Start(List<int> parts, string locale) {
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
            Console.Clear();

            if (parts.Contains(1)) {
                Part1_ActivatingWindows();
            }
            
            Console.Exit(0, false);
        }

        /// <summary>
        /// Runs part 0. This will give the user the chance of selecting a new language.
        /// </summary>
        private void Part0_SelectLanguage() {
            Console.Clear();
            Console.Title(Strings.LanguageSelectionTitle);

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

        /// <summary>
        /// Runs part 1. This will give the user the chance to activate Windows.
        /// </summary>
        private void Part1_ActivatingWindows(bool gotoInput = false) {
            Console.Clear();
            Console.Title(Strings.Part1_TitleWindow);
            Console.Write("", Strings.Part1_Title);
            Console.Write("", "");

            if (!Windows.IsActivated()) {
                Console.Write(Strings.Part1_AlreadyActivated);
                Thread.Sleep(Console.GetReadingTime(Strings.Part1_AlreadyActivated));
                return;
            } else {
                Console.Write(Strings.Part1_Intro);

                var hasActivationKeySelection = Console.CreateSelection(Strings.Part1_HasActivationKey_Selection, new List<ConsoleHelper.SelectionOption>() {
                    new ConsoleHelper.SelectionOption(1, Strings.Part1_HasActivationKey_Selection_Yes),
                    new ConsoleHelper.SelectionOption(2, Strings.Part1_HasActivationKey_Selection_No),
                    new ConsoleHelper.SelectionOption(3, Strings.Part1_HasActivationKey_Selection_Skip)
                });

                switch (hasActivationKeySelection.Number) {
                    case 1:
                        var activationKey = Console.CreateQuestion(Strings.Part1_ActivationKey_Request, Strings.Part1_ActivationKey_Request_Name);
                        Console.Write("", "");
                       
                        Console.Write(Strings.Part1_ActivationKeyInstallation_Installing);
                        //Install the activation key
                        Console.Write("", "");

                        var isKmsSelection = Console.CreateSelection(Strings.Part1_KmsServer_Selection, new List<ConsoleHelper.SelectionOption>() {
                            new ConsoleHelper.SelectionOption(1, Strings.Part1_KmsServer_Selection_DontKnow),
                            new ConsoleHelper.SelectionOption(2, Strings.Part1_KmsServer_Selection_Yes),
                            new ConsoleHelper.SelectionOption(3, Strings.Part1_KmsServer_Selection_No)
                        });

                        switch (isKmsSelection.Number) {
                            case 1:
                                Console.Write(Strings.Part1_NotUsingKmsServer);
                                Thread.Sleep(200);
                                break;

                            case 2:
                                var kmsDomain = Console.CreateQuestion(Strings.Part1_KmsServer_Request, Strings.Part1_KmsServer_Request_Name);
                                
                                // Set KMS server
                                
                                break;

                            case 3:
                                Console.Write(Strings.Part1_NotUsingKmsServer);
                                Thread.Sleep(200);
                                break;
                        }

                        // Activate Windows

                        break;

                    case 2:
                        Console.Write(Strings.Part1_WindowsEditions);
                        Console.Write("", "");
                        Console.Write(Strings.Part1_WindowsEditions_Overview);
                        Console.Write(Strings.Part1_WindowsEditions_Overview_Home);
                        Console.Write(Strings.Part1_WindowsEditions_Overview_Pro);
                        Console.Write(Strings.Part1_WindowsEditions_Overview_ProForWorkstations);
                        Console.Write("", "");
                        Console.Write(Strings.Part1_WindowsEditions_SupportNote);
                        Console.Write("", "");
                        Console.Write(Strings.Part1_WindowsEditions_LearnMore);
                        Console.Write("", "");

                        var windowsEditionSelection = Console.CreateSelection(Strings.Part1_WindowsEditions_Selection, new List<ConsoleHelper.SelectionOption>() {
                            new ConsoleHelper.SelectionOption(1, Strings.Part1_WindowsEditions_Selection_Home),
                            new ConsoleHelper.SelectionOption(2, Strings.Part1_WindowsEditions_Selection_Pro),
                            new ConsoleHelper.SelectionOption(3, Strings.Part1_WindowsEditions_Selection_ProForWorkstations),
                            new ConsoleHelper.SelectionOption(3, Strings.Part1_WindowsEditions_Selection_Skip)
                        });

                        switch (windowsEditionSelection.Number) {
                            case 1:
                                Console.Write(Strings.Part1_WindowsEditions_OpenMessage.Replace("{edition}", "Home"));
                                
                                var contOrInstrKey_H = Console.ReadKeys(Strings.Part1_WindowsEditions_ContinueOrInstructions, new List<System.ConsoleKey>() {
                                    System.ConsoleKey.Enter,
                                    System.ConsoleKey.D1
                                });
                                if (contOrInstrKey_H == System.ConsoleKey.D1) {
                                    Console.Write(Strings.Part1_WindowsEditions_OpeningInstructions);
                                    //Open detailed instructions
                                }

                                string linkH = "";
                                Console.Write(Strings.Part1_WindowsEditions_OpeningLink.Replace("{link}", linkH));
                                Console.RunCommand("start " + linkH);

                                var boughtOrSkipKeyH = Console.ReadKeys(Strings.Part1_WindowsEditions_BoughtOrSkip, new List<System.ConsoleKey>() {
                                    System.ConsoleKey.Enter,
                                    System.ConsoleKey.Escape
                                });
                                switch (boughtOrSkipKeyH) {
                                    case System.ConsoleKey.Escape:
                                        Console.Write(Strings.Part1_UserSkippedActivation);
                                        Thread.Sleep(Console.GetReadingTime(Strings.Part1_UserSkippedActivation));
                                        return;

                                    case System.ConsoleKey.Enter:
                                        Part1_ActivatingWindows(true);
                                        break;
                                }
                                break;

                            case 2:
                                Console.Write(Strings.Part1_WindowsEditions_OpenMessage.Replace("{edition}", "Home"));

                                var contOrInstrKeyP = Console.ReadKeys(Strings.Part1_WindowsEditions_ContinueOrInstructions, new List<System.ConsoleKey>() {
                                    System.ConsoleKey.Enter,
                                    System.ConsoleKey.D1
                                });
                                if (contOrInstrKeyP == System.ConsoleKey.D1) {
                                    Console.Write(Strings.Part1_WindowsEditions_OpeningInstructions);
                                    //Open detailed instructions
                                }

                                string linkP = "";
                                Console.Write(Strings.Part1_WindowsEditions_OpeningLink.Replace("{link}", linkP));
                                Console.RunCommand("start " + linkP);

                                var boughtOrSkipKeyP = Console.ReadKeys(Strings.Part1_WindowsEditions_BoughtOrSkip, new List<System.ConsoleKey>() {
                                    System.ConsoleKey.Enter,
                                    System.ConsoleKey.Escape
                                });
                                switch (boughtOrSkipKeyP) {
                                    case System.ConsoleKey.Escape:
                                        Console.Write(Strings.Part1_UserSkippedActivation);
                                        Thread.Sleep(Console.GetReadingTime(Strings.Part1_UserSkippedActivation));
                                        return;

                                    case System.ConsoleKey.Enter:
                                        Part1_ActivatingWindows(true);
                                        break;
                                }
                                break;

                            case 3:
                                break;

                            case 4:
                                break;

                            default:
                                Console.WriteError(Strings.Part1_WindowsEditions_Selection_Error);
                                Thread.Sleep(Console.GetReadingTime(Strings.Part1_WindowsEditions_Selection_Error));
                                break;
                        }

                        break;

                    case 3:
                        Console.Write(Strings.Part1_UserSkippedActivation);
                        Thread.Sleep(Console.GetReadingTime(Strings.Part1_UserSkippedActivation));
                        break;
                }
            }
            Console.Clear();
        }
    }
}
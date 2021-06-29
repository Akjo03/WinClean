﻿using WinClean.resources;

using System.Collections.Generic;
using System.Threading;

namespace WinClean {
    /// <summary>
    /// This class is only used for parsing command line arguments
    /// </summary>
    public class ArgumentParser {
        
        private ConsoleHelper consoleRef;
        private LocaleHelper localeRef;


        public ArgumentParser(ConsoleHelper consoleRef, LocaleHelper localeRef) {
            this.consoleRef = consoleRef;
            this.localeRef = localeRef;
        }

        /// <summary>
        /// Parses the given command line arguments
        /// </summary>
        /// <param name="args">The arguments that should be parsed</param>
        /// <returns>
        /// A list of parts and the locale that WinClean should be ran on.
        /// </returns>
        public (List<int>, string) Parse(string[] args) {
            if (args.Length <= 0) {
                // If no arguments given run all parts and start with no locale
                return (WinClean.availableParts, null);
            } else {
                List<int> partsResult = new List<int>();
                string localeResult = null;

                // If arguments given go through all of them
                foreach (string arg in args) {
                    if (arg.StartsWith("-parts:")) {
                        // If argument "parts" given read them
                        string argString = arg[7..];
                        string[] partsInput = argString.Split(",");
                        if (string.IsNullOrWhiteSpace(string.Join("", partsInput))) {
                            continue;
                        }

                        // Go through each part given as input and validate them
                        bool errorOnArgParse = false;
                        foreach (string partInput in partsInput) {
                            if (int.TryParse(partInput, out int partNum)) {
                                if (WinClean.availableParts.Contains(partNum)) {
                                    // If the given part is available add the part to the list
                                    partsResult.Add(partNum);
                                } else {
                                    // Given part not in the list of available parts.
                                    errorOnArgParse = true;
                                    consoleRef.WriteError(Strings.ArgParse_PartNotFound.Replace("{part_name}", partInput.Trim()));
                                }
                            } else {
                                // Given part is not numerical
                                errorOnArgParse = true;
                                consoleRef.WriteError(Strings.ArgParse_PartNotNumerical.Replace("{part_name}", partInput.Trim()));
                            }
                        }
                        if (errorOnArgParse) {
                            // If any error occured above exit
                            consoleRef.EnterToContinue(Strings.EnterToExit);
                            consoleRef.Exit(-1, false);
                        }
                        if (partsResult.Contains(0) && localeResult != null) {
                            consoleRef.WriteWarn(Strings.ArgParse_SkippingLanguageSelection);
                            Thread.Sleep(consoleRef.GetReadingTime(Strings.ArgParse_SkippingLanguageSelection));
                        }
                    } else if (arg.StartsWith("-locale:")) {
                        // If argument "locale" was given get it
                        string localeInput = arg[8..].Trim();

                        // Validate the locale given
                        if (WinClean.availableLocale.Contains(localeInput.Trim())) {
                            // If the given locale is available set it as the result
                            localeResult = localeInput.Trim();
                        } else {
                            // Given locale is not available
                            consoleRef.WriteError(Strings.ArgParse_LocaleNotFound.Replace("{locale}", localeInput));
                            consoleRef.EnterToContinue(Strings.EnterToExit);
                            consoleRef.Exit(-1, false);
                        }
                    } else if (arg == "-h" || arg == "-?") {
                        // If locale already set through arguments set it
                        if (localeResult != null) {
                            localeRef.SetLocale(localeResult);
                            consoleRef.Clear();
                        }

                        // Write the Help for WinClean arguments to the console and exit without clearing the console
                        consoleRef.Write(Strings.ArgParse_Help);
                        consoleRef.Exit(0, false, false);
                    } else {
                        // If argument is invalid print an error
                        consoleRef.WriteError(Strings.ArgParse_Error);
                        consoleRef.EnterToContinue(Strings.EnterToExit);
                        consoleRef.Exit(-1, false);
                    }
                }
                return (partsResult, localeResult);
            }
        }
    }
}
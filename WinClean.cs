using WinClean.resources;

using System;

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
            Console.WriteLine(Strings.WelcomeMessage);
            Console.WriteLine(Strings.EarlyDevelopmentMessage);
            Console.ReadLine();
        }
    }
}
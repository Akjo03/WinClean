using System.Collections.Generic;

namespace WinClean {
    public class ArgumentParser {
        ConsoleHelper consoleRef;

        public ArgumentParser(ConsoleHelper consoleRef) {
            this.consoleRef = consoleRef;
        }

        public (List<int>, string) Parse(string[] args) {
            if (args.Length <= 0) {
                return (new List<int>() { 0 }, null);
            } else {
                foreach (string arg in args) {
                    consoleRef.Write(arg);
                }
                return (new List<int>() { 0 }, null);
            }
        }
    }
}
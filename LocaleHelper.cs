using System.Globalization;
using System.Threading;

using static WinClean.ConsoleHelper;

namespace WinClean {
    public class LocaleHelper {
        private ConsoleHelper ConsoleRef { get; }

        private string CurrentLocale;

        public LocaleHelper(ConsoleHelper consoleRef) {
            this.ConsoleRef = consoleRef;
        }

        public void SetLocale(string locale) {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
            ConsoleRef.Write("Changed language to " + locale);
            CurrentLocale = locale;
        }

        public string GetLocale() {
            return CurrentLocale;
        }
    }
}

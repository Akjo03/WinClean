using System.Globalization;
using System.Threading;

namespace WinClean {
    /// <summary>
    /// Helps to set the current locale
    /// </summary>
    public class LocaleHelper {
        private ConsoleHelper ConsoleRef { get; }

        private string CurrentLocale;

        public LocaleHelper(ConsoleHelper consoleRef) {
            this.ConsoleRef = consoleRef;
        }

        /// <summary>
        /// Sets the current locale
        /// </summary>
        /// <param name="locale">The locale to set to</param>
        public void SetLocale(string locale) {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
            ConsoleRef.Write("Changed language to " + locale);
            CurrentLocale = locale;
        }

        /// <summary>
        /// Gets the current locale
        /// </summary>
        /// <returns>The current locale</returns>
        public string GetLocale() {
            return CurrentLocale;
        }
    }
}

using System.Globalization;
using System.Threading;

using static WinClean.ConsoleHelper;

namespace WinClean {
    public class LocaleHelper {
        private ConsoleHelper ConsoleRef { get; }

        public LocaleHelper(ConsoleHelper consoleRef) {
            this.ConsoleRef = consoleRef;
        }

        public void SetLang(string locale) {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
            ConsoleRef.Write("Changed language to " + locale);
        }
    }
}

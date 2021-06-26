using System.Globalization;
using System.Threading;

using static WinClean.ConsoleHelper;

namespace WinClean {
    public class LocaleHelper {
        private ConsoleHelper consoleRef;

        public LocaleHelper(ConsoleHelper consoleRef) {
            this.consoleRef = consoleRef;
        }

        public void SetLang(string locale) {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
            consoleRef.Write("Changed language to " + locale);
        }
    }
}

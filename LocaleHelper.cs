using System.Globalization;
using System.Threading;

using static WinClean.ConsoleHelper;

namespace WinClean {
    public class LocaleHelper {
        public static void SetLang(string locale) {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
            ConsoleWrite("Changed language to " + locale);
        }
    }
}

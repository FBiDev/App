using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace App.Core
{
    public static class LanguageManager
    {
        #region Language
        public static readonly CultureInfo CultureBrazil = SetCultureDateNames(CultureID.Brazil_Portuguese);
        public static readonly CultureInfo CultureUSA = SetCultureDateNames(CultureID.UnitedStates_English);

        private static CultureInfo language = SetCultureDateNames(CultureID.UnitedStates_English);
        private static RegionInfo country;

        public static CultureInfo LanguageNumbers { get; private set; }

        public static string CurrencySymbol
        {
            get { return language.NumberFormat.CurrencySymbol; }
        }

        public static string CurrencyGroupSeparator
        {
            get { return language.NumberFormat.CurrencyGroupSeparator; }
        }

        public static string CurrencyDecimalSeparator
        {
            get { return language.NumberFormat.CurrencyDecimalSeparator; }
        }

        public static string NumberDecimalSeparator
        {
            get { return language.NumberFormat.NumberDecimalSeparator; }
        }
        #endregion

        public static CultureInfo GetLanguage()
        {
            return language;
        }

        public static void SetLanguage(CultureID cultureID)
        {
            var cultureName = Convert.ToInt32(cultureID);
            language = new CultureInfo(cultureName);

            SetCultureDateNames(language);

            // Culture for any thread
            CultureInfo.DefaultThreadCurrentCulture = language;

            // Culture for UI in any thread
            CultureInfo.DefaultThreadCurrentUICulture = language;

            country = new RegionInfo(Thread.CurrentThread.CurrentUICulture.LCID);
        }

        public static void SetLanguageNumbers(CultureID name)
        {
            LanguageNumbers = new CultureInfo(Convert.ToInt32(name));
        }

        public static CultureInfo SetCultureDateNames(CultureID cultureID)
        {
            var cultureName = Convert.ToInt32(cultureID);
            var language = new CultureInfo(cultureName);
            SetCultureDateNames(language);

            return language;
        }

        public static void SetCultureDateNames(CultureInfo culture)
        {
            // Change Culture Info Month names
            culture.DateTimeFormat.MonthNames = culture.DateTimeFormat.MonthNames.Select(m => culture.TextInfo.ToTitleCase(m)).ToArray();
            culture.DateTimeFormat.MonthGenitiveNames = culture.DateTimeFormat.MonthGenitiveNames.Select(m => culture.TextInfo.ToTitleCase(m)).ToArray();
            culture.DateTimeFormat.AbbreviatedMonthNames = culture.DateTimeFormat.AbbreviatedMonthNames.Select(m => culture.TextInfo.ToTitleCase(m)).ToArray();

            culture.DateTimeFormat.DayNames = culture.DateTimeFormat.DayNames.Select(d => culture.TextInfo.ToTitleCase(d)).ToArray();

            if (culture.LCID == (int)CultureID.Brazil_Portuguese)
            {
                culture.DateTimeFormat.DayNames = culture.DateTimeFormat.DayNames.Select(d => d.Replace("-Feira", string.Empty)).ToArray();
            }
        }
    }
}
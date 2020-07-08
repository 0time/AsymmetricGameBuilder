using System.Collections.Generic;
using UnityEngine;

namespace me.zti.localizations {
  public class LocalizationMap {
    private const string DEFAULT_LOCALE = "en-US";
    private const bool DEFAULT_THROW_EXCEPTION_IF_MISSING_BASE_STRING = true;

#if UNITY_EDITOR
    private const bool DEFAULT_THROW_EXCEPTION_IF_MISSING_LOCALE = true;
    private const bool USE_DEFAULT_LOCALE = false;
#else
    // Default settings are prod settings
    private const bool DEFAULT_THROW_EXCEPTION_IF_MISSING_LOCALE = false;
    private const bool USE_DEFAULT_LOCALE = true;
#endif

    private Dictionary<string, ILocalization> mLocalizations;

    private bool mThrowExceptionIfMissingLocale;
    private bool mThrowExceptionIfMissingBaseString;
    private bool mDefaultToEnUS;

    public LocalizationMap(
      bool pThrowExceptionIfMissingLocale = DEFAULT_THROW_EXCEPTION_IF_MISSING_LOCALE,
      bool pThrowExceptionIfMissingBaseString = DEFAULT_THROW_EXCEPTION_IF_MISSING_BASE_STRING,
      bool pDefaultToEnUS = USE_DEFAULT_LOCALE
    ) {
      mThrowExceptionIfMissingLocale = pThrowExceptionIfMissingLocale;
      mThrowExceptionIfMissingBaseString = pThrowExceptionIfMissingBaseString;
      mDefaultToEnUS = pDefaultToEnUS;

      mLocalizations = new Dictionary<string, ILocalization>();
    }

    public void addLocalization(string baseString, string locale, string localization) {
      addLocalization(baseString, new string[] {locale}, new string[] {localization});
    }

    public void addLocalization(string baseString, string[] locales, string[] localizations) {
      Localization localization = new Localization(locales, localizations);

      mLocalizations.Add(baseString, localization);
    }

    public string translate(string baseString, string locale) {
      string errorString = "";
      ILocalization localization;

      if (!mLocalizations.ContainsKey(baseString)) {
        errorString = "Missing base string " + baseString + ".";

        if (mThrowExceptionIfMissingBaseString) {
          // FIXME: More specific exception type
          throw new System.Exception(errorString);
        } else {
          Debug.LogWarning(errorString);

          return baseString;
        }
      } else {
        localization = mLocalizations[baseString];

        if (!localization.ContainsLocale(locale)) {
          errorString = "Missing localization for locale " + locale + " and base string " + baseString + ".";

          if (mThrowExceptionIfMissingLocale) {
            // FIXME: More specific exception type
            throw new System.Exception(errorString);
          } else {
            Debug.LogWarning(errorString);

            if (mDefaultToEnUS && localization.ContainsLocale(DEFAULT_LOCALE)) {
              return mLocalizations[baseString].GetLocalizedString(DEFAULT_LOCALE, baseString);
            } else {
              return baseString;
            }
          }
        } else {
          return mLocalizations[baseString].GetLocalizedString(locale, baseString);
        }
      }
    }

    public string toString() {
      List<string> locales = new List<string>();
      List<string> rows = new List<string>();

      string row;
      string final;

      foreach (KeyValuePair<string, ILocalization> kv in mLocalizations) {
        foreach (string locale in kv.Value.locales) {
          if (!locales.Contains(locale)) {
            locales.Add(locale);
          }
        }
      }

      foreach (KeyValuePair<string, ILocalization> kv in mLocalizations) {
        row = kv.Key;

        foreach (string locale in locales) {
          row += "," + kv.Value.GetLocalizedString(locale, "");
        }

        rows.Add(row);
      }

      final = "Base String";

      foreach (string locale in locales) {
        final += "," + locale;
      }

      foreach (string each in rows) {
        final += "\n" + each;
      }

      return final;
    }
  }
}

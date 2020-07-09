using System.Collections.Generic;
using UnityEngine;

namespace me.zti.localizations {
  public class LocalizationMap {
    public enum MissingLocalesConfiguration {
      UseBaseString = 0,
      UseDefaultLocale = 1,
      ThrowError = 2
    }

    private const string DEFAULT_LOCALE = "en-US";

    // Default settings are prod settings
    private const bool DEFAULT_THROW_EXCEPTION_IF_MISSING_BASE_STRING = true;

#if UNITY_EDITOR
    private const MissingLocalesConfiguration DEFAULT_MISSING_LOCALES_CONFIGURATION = MissingLocalesConfiguration.ThrowError;
#else
    private const MissingLocalesConfiguration DEFAULT_MISSING_LOCALES_CONFIGURATION = MissingLocalesConfiguration.UseDefaultLocale;
#endif

    private Dictionary<string, ILocalization> mLocalizations;

    private MissingLocalesConfiguration mMissingLocalesConfiguration;
    private bool mThrowExceptionIfMissingBaseString;

    public LocalizationMap(
      MissingLocalesConfiguration pMissingLocalesConfiguration = DEFAULT_MISSING_LOCALES_CONFIGURATION,
      bool pThrowExceptionIfMissingBaseString = DEFAULT_THROW_EXCEPTION_IF_MISSING_BASE_STRING
    ) {
      mMissingLocalesConfiguration = pMissingLocalesConfiguration;
      mThrowExceptionIfMissingBaseString = pThrowExceptionIfMissingBaseString;

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

          if (mMissingLocalesConfiguration == MissingLocalesConfiguration.ThrowError) {
            // FIXME: More specific exception type
            throw new System.Exception(errorString);
          } else {
            Debug.LogWarning(errorString);

            if (localization.HasFallbackForLocale(locale)) {
              return localization.GetFallbackForLocale(locale);
            } else if (mMissingLocalesConfiguration == MissingLocalesConfiguration.UseDefaultLocale && localization.ContainsLocale(DEFAULT_LOCALE)) {
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

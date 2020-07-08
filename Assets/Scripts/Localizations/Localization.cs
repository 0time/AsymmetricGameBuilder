using System.Collections.Generic;
using System.Linq;

namespace me.zti.localizations {
  public class Localization : ILocalization {
    protected Dictionary<string, string> mDictionary;

    public Localization(string[] pLocales, string[] pLocalizations) {
      mDictionary = new Dictionary<string, string>();

      if (pLocales.Length != pLocalizations.Length) {
        throw new System.Exception("FIXME: This error indicates a mismatch between locales and localizations");
      }

      for (int i = 0; i < pLocales.Length; ++i) {
        mDictionary.Add(pLocales[i], pLocalizations[i]);
      }
    }

    public bool ContainsLocale(string pLocale) {
      return mDictionary.ContainsKey(pLocale);
    }

    public string GetLocalizedString(string pLocale, string defaultIfMissing = null) {
      // TODO: Fixme to support best fit (i.e. if locale is es-EC but no es-EC localization exists, use es-ES or something)
      if (!mDictionary.ContainsKey(pLocale)) {
        return defaultIfMissing;
      }

      return mDictionary[pLocale];
    }

    public string[] locales {
      get {
        return mDictionary.Keys.ToArray();
      }
    }
  }
}

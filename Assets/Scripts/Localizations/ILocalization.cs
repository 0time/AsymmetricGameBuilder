namespace me.zti.localizations {
  public interface ILocalization {
    bool ContainsLocale(string pLocale);
    string GetLocalizedString(string pLocale, string defaultIfMissing);
    string[] locales {get;}
    bool HasFallbackForLocale(string pLocale);

    string GetFallbackForLocale(string pLocale);
  }
}

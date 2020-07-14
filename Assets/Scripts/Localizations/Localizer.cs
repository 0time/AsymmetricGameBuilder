using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using me.zti.helpers;
using me.zti.ui;

namespace me.zti.localizations {
  public class Localizer {
    private Dictionary<ILocalizable, string> mBaseStrings;
    private LocalizationMap mLocalizationMap;

    public enum OverrideLocalesConfiguration {
      Unchanged = -1,
      UseBaseString = 0,
      UseDefaultLocale = 1,
      ThrowErrorOnlyIfNoFallback = 2,
      ThrowErrorAlways = 3
    }

    private string mLocale = "";

    public string locale {
      get {
        if (mLocale != "" && mLocale != null) {
          return mLocale;
        } else {
          // TODO: Fixme to be dynamic based on something from the application context
          return "en-US";
        }
      }
    }

    public Localizer(string pLocale = "", OverrideLocalesConfiguration overrideLocalesConfiguration = OverrideLocalesConfiguration.Unchanged, TextAsset stringTranslations = null) {
      bool overrideable = false;

#if UNITY_EDITOR
      overrideable = true;
#endif

      LocalizationMap localizationMap;

      if (overrideable && overrideLocalesConfiguration != OverrideLocalesConfiguration.Unchanged) {
        localizationMap = new LocalizationMap((LocalizationMap.MissingLocalesConfiguration) overrideLocalesConfiguration);
      } else {
        localizationMap = new LocalizationMap();
      }

      initialize(new LocalizationMap(), pLocale, stringTranslations);
    }

    public Localizer(LocalizationMap pLocalizationMap, string pLocale = "", TextAsset stringTranslations = null) {
      initialize(pLocalizationMap, pLocale, stringTranslations);
    }

    public void initialize(LocalizationMap pLocalizationMap, string pLocale = "", TextAsset stringTranslations = null) {
      if (pLocale != "" && pLocale != null) {
        mLocale = pLocale;
      }

      mLocalizationMap = pLocalizationMap;

      Debug.Log("The locale for this session will be: " + locale);

      mBaseStrings = new Dictionary<ILocalizable, string>();
      fetchLocalizations(stringTranslations);
      fetchLocalizableObjects();
    }

    private void fetchLocalizations(TextAsset stringTranslations = null) {
      string[] locales = new string[] {};
      string[] localizations = new string[] {};
      int index = 0;

      Csv.DeserializeWithHeader(
        new StringReader(stringTranslations.text),
        (Dictionary<string, string> row) => {
          if (locales.Length != row.Count - 1) {
            System.Array.Resize(ref locales, row.Count - 1);
          }

          if (localizations.Length != row.Count - 1) {
            System.Array.Resize(ref localizations, row.Count - 1);
          }

          foreach (string key in row.Keys) {
            if (key != "Base String") {
              locales[index] = key;
              localizations[index] = row[key];

              ++index;
            }
          }

          mLocalizationMap.addLocalization(row["Base String"], locales, localizations);
        });
    }

    public void track(ILocalizable obj) {
      if (obj.baseString == null) {
        throw new System.Exception("Base string cannot be null, GameObject: " + ((Component)obj).gameObject.name);
      }

      if (!mBaseStrings.ContainsKey(obj)) {
        mBaseStrings.Add(obj, obj.baseString);
      } else {
        // Update the base string
        mBaseStrings[obj] = obj.baseString;
      }
    }

    private void fetchLocalizableObjects() {
      foreach(ILocalizable ea in InterfaceHelper.FindObjects<ILocalizable>()) {
        track(ea);
      }
    }

    public void executeLocalizations() {
      foreach (KeyValuePair<ILocalizable, string> kp in mBaseStrings) {
        Debug.Log(kp.Value + " " + locale + " ");
        kp.Key.text = mLocalizationMap.translate(kp.Value, locale);
      }
    }
  }
}

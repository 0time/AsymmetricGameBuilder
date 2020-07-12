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
      ThrowError = 2
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

    public Localizer(string pLocale = "", OverrideLocalesConfiguration overrideLocalesConfiguration = OverrideLocalesConfiguration.Unchanged) {
      bool overrideable = false;

#if UNITY_EDITOR
      overrideable = true;
#endif

      if (overrideable && overrideLocalesConfiguration != OverrideLocalesConfiguration.Unchanged) {
        initialize(new LocalizationMap((LocalizationMap.MissingLocalesConfiguration) overrideLocalesConfiguration), pLocale);
      } else {
        initialize(new LocalizationMap(), pLocale);
      }
    }

    public Localizer(LocalizationMap pLocalizationMap, string pLocale = "") {
      initialize(pLocalizationMap, pLocale);
    }

    public void initialize(LocalizationMap pLocalizationMap, string pLocale = "") {
      if (pLocale != "" && pLocale != null) {
        mLocale = pLocale;
      }

      mLocalizationMap = pLocalizationMap;

      Debug.Log("The locale for this session will be: " + locale);

      mBaseStrings = new Dictionary<ILocalizable, string>();
      fetchLocalizations();
      fetchLocalizableObjects();
    }

    private void fetchLocalizations() {
      string path = "Assets/Data/Localizations/Strings.csv";
      string[] locales = new string[] {};
      string[] localizations = new string[] {};
      int index = 0;

      Csv.DeserializeWithHeader(new StreamReader(path), (Dictionary<string, string> row) => {
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

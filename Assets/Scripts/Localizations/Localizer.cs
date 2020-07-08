using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using me.zti.helpers;
using me.zti.ui;

namespace me.zti.localizations {
  public class Localizer {
    private Dictionary<ILocalizable, string> mBaseStrings;
    private LocalizationMap mLocalizationMap;

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

    public Localizer(string pLocale = "") {
      initialize(new LocalizationMap(), pLocale);
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
      string[] locales = new string[] {
        "en-US",
        "es-EC"
      };
      string[] localizations = new string[] {
        "Class Consciousness",
        "Ecuador's Class Consciousness String That I Don't Know"
      };

      mLocalizationMap.addLocalization("CLASS_CONSCIOUSNESS_BAR_TITLE", locales, localizations);
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

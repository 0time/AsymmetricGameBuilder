using UnityEngine;
using UnityEngine.UI;

namespace me.zti.localizations {
  public class LocalizerMonoBehaviour : MonoBehaviour {
    private Localizer localizer;

    [Tooltip("Recommended locales: en-US (English US), en-UK (English UK), es-ES (Spanish Spain), it-IT (Italian Italy)")]
    public string Locale = "";

    [Tooltip("Only works for Editor, disabled for builds")]
    public Localizer.OverrideLocalesConfiguration overrideLocalesConfiguration = Localizer.OverrideLocalesConfiguration.Unchanged;

    [Tooltip("The file to use for string translations")]
    public TextAsset stringTranslations;

    void Awake() {
      localizer = new Localizer(Locale, overrideLocalesConfiguration, stringTranslations);

      localizer.executeLocalizations();
    }
  }
}

using UnityEngine;
using UnityEngine.UI;

namespace me.zti.localizations {
  public class LocalizerMonoBehaviour : MonoBehaviour {
    private Localizer localizer;

    public string Locale = "";

    [Tooltip("Only works for Editor, disabled for builds")]
    public Localizer.OverrideLocalesConfiguration overrideLocalesConfiguration = Localizer.OverrideLocalesConfiguration.Unchanged;

    void Awake() {
      localizer = new Localizer(Locale, overrideLocalesConfiguration);

      localizer.executeLocalizations();
    }
  }
}

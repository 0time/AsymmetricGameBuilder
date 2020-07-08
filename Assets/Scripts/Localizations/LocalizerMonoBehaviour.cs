using UnityEngine;
using UnityEngine.UI;

namespace me.zti.localizations {
  public class LocalizerMonoBehaviour : MonoBehaviour {
    private Localizer localizer;

    public string Locale = "";

    void Awake() {
      localizer = new Localizer(Locale);

      localizer.executeLocalizations();
    }
  }
}

using UnityEngine;
using UnityEngine.UI;

using me.zti.localizations;
using me.zti.ui;

namespace me.zte.ui {
  public class LocalizableText : MonoBehaviour, ILocalizable {
    public string LocalizedPlaceholder = "";
    public Text TextComponent;

    public string baseString {
      get {
        string str;

        if (LocalizedPlaceholder != null && LocalizedPlaceholder != "") {
          str = LocalizedPlaceholder;
        } else {
          str = TextComponent.text;
        }

        if (str == null) {
          Debug.Log("Eh? BaseString: " + str + " LocalizedPlaceholder: " + LocalizedPlaceholder);
        }

        return str;
      }
    }

    public string text {
      get {
        return TextComponent.text;
      }
      set {
        TextComponent.text = value;
      }
    }

    void Awake() {
      if (TextComponent == null) {
        TextComponent = this.gameObject.GetComponent<Text>();
      }
    }
  }
}

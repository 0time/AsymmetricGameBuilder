using UnityEngine;
using UnityEngine.UI;

namespace me.zti.ui {
  public class GraphicalProgressBar : ProgressBar {
    public RectTransform barToExpand;
    public Text progressBarTitle;
    public float maxWidth;

    public void addValue(float v) {
      this.setValue(this.getValue() + v);
    }

    protected new void Awake() {
      base.Awake();
    }
  }
}

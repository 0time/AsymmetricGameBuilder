using UnityEngine;

namespace me.zti.ui {
  public class ProgressBar : MonoBehaviour, ISupportingProgressBars {
    public float InitialMinimum;
    public float InitialValue;
    public float InitialMaximum;

    private float mMinimum;
    private float mValue;
    private float mMaximum;

    protected void Awake() {
      mMinimum = InitialMinimum;
      mValue = InitialValue;
      mMaximum = InitialMaximum;
    }

    public float getPercent() {
      float percent = 100 * (mValue - mMinimum) / (mMaximum - mMinimum);

      if (percent > 100f) {
        return 100f;
      } else if (percent < 0f) {
        return 0f;
      } else {
        return percent;
      }
    }

    public float getMaximum() {
      return mMaximum;
    }

    public float getMinimum() {
      return mMinimum;
    }

    public float getValue() {
      return mValue;
    }

    public void setMaximum(float pMaximum) {
      mMaximum = pMaximum;
    }

    public void setMinimum(float pMinimum) {
      mMinimum = pMinimum;
    }

    public void setValue(float pValue) {
      mValue = pValue;
    }
  }
}

namespace me.zti.ui {
  public class ClassConsciousnessBar : ISupportingProgressBars {
    private float mValue = 0f;
    private float mMaximum = 100f;
    private float mMinimum = 0f;

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

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace me.zti.ui {
  public class ProgressBarTests {
    [Test]
    public void When_ValueIs50f_Expect_50Percent() {
      GameObject go = new GameObject();
      ProgressBar ccb = go.AddComponent<ProgressBar>();

      ccb.setMinimum(0f);
      ccb.setValue(50f);
      ccb.setMaximum(100f);

      Assert.AreEqual(50f, ccb.getPercent());
    }

    [Test]
    public void When_PercentIsBelow0_Expect_0Percent() {
      GameObject go = new GameObject();
      ProgressBar ccb = go.AddComponent<ProgressBar>();

      ccb.setMinimum(0f);
      ccb.setValue(-5f);
      ccb.setMaximum(100f);

      Assert.AreEqual(0f, ccb.getPercent());
    }

    [Test]
    public void When_Min100Value140Max300_Expect_20Percent() {
      GameObject go = new GameObject();
      ProgressBar ccb = go.AddComponent<ProgressBar>();

      ccb.setMinimum(100f);
      ccb.setValue(140f);
      ccb.setMaximum(300f);

      Assert.AreEqual(20f, ccb.getPercent());
    }

    [Test]
    public void When_PercentIsAbove100_Expect_100Percent() {
      GameObject go = new GameObject();
      ProgressBar ccb = go.AddComponent<ProgressBar>();

      ccb.setMinimum(0f);
      ccb.setValue(500f);
      ccb.setMaximum(100f);

      Assert.AreEqual(100f, ccb.getPercent());
    }
  }
}

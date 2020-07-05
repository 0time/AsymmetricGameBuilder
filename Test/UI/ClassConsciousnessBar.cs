using NUnit.Framework;

namespace me.zti.ui.tests {
  [TestFixture]
  public class ClassConsciousnessBarTests {
    [TestCase]
    public void When_ValueIs50f_Expect_50Percent() {
      ClassConsciousnessBar ccb = new ClassConsciousnessBar();

      ccb.setValue(50f);

      Assert.AreEqual(50f, ccb.getPercent());
    }

    [TestCase]
    public void When_PercentIsBelow0_Expect_0Percent() {
      ClassConsciousnessBar ccb = new ClassConsciousnessBar();

      ccb.setValue(-5f);

      Assert.AreEqual(0f, ccb.getPercent());
    }

    [TestCase]
    public void When_MinValueMaxIsM100V120M200_Expect_20Percent() {
      ClassConsciousnessBar ccb = new ClassConsciousnessBar();

      ccb.setMinimum(100f);
      ccb.setValue(120f);
      ccb.setMaximum(200f);

      Assert.AreEqual(20f, ccb.getPercent());
    }

    [TestCase]
    public void When_PercentIsAbove100_Expect_100Percent() {
      ClassConsciousnessBar ccb = new ClassConsciousnessBar();

      ccb.setValue(500f);

      Assert.AreEqual(100f, ccb.getPercent());
    }
  }
}

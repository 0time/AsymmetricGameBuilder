using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using me.zti.ui;

namespace me.zti.localizations {
  public class LocalizationMapTests {
    private class LocalizableMock : ILocalizable {
      public LocalizableMock(string pBaseString) {
        baseString = pBaseString;
      }

      public string baseString {
        get;
        set;
      }

      public string text {
        get;
        set;
      }
    }

    private const string ABSENT_BASE_STRING = "Absent";
    private const string PRESENT_1_BASE_STRING = "Present 1 Base String";
    private const string PRESENT_1_EN_US_STRING = "Present 1 en-US String";
    private const string PRESENT_1_ES_ES_STRING = "Present 1 es-ES String";
    private const string PRESENT_2_BASE_STRING = "Present 2 Base String";
    private const string PRESENT_2_EN_US_STRING = "Present 2 en-US String";
    private const string PRESENT_2_ES_ES_STRING = "Present 2 es-ES String";
    private const string EN_US_ONLY_BASE_STRING = "en-US Only Base String";
    private const string EN_US_STRING = "en-US String";
    private const string ES_ES_ONLY_BASE_STRING = "es-ES Only Base String";
    private const string ES_ES_STRING = "es-ES String";
    private const string LOCALE_EN_US = "en-US";
    private const string LOCALE_ES_ES = "es-ES";
    private const string LOCALE_ABSENT = "absent";

    /*
     * TODO: Think about how to test defaults using this one?
    private LocalizationMap getSample() {
      LocalizationMap tmap = new LocalizationMap();

      initialize(tmap);
    }
    */

    // A nullable bool
    private enum nBool {
      setFalse = 0,
      setTrue = 1,
      unset = 2
    }

    private LocalizationMap[] getSamples(nBool throwForLocale, nBool throwForBase, nBool defaultEn) {
      List<LocalizationMap> bList = new List<LocalizationMap>();

      bool[] localeSet = throwForLocale == nBool.unset
        ? new bool[] {false, true}
        : new bool[] {throwForLocale == nBool.setTrue};

      bool[] baseSet = throwForBase == nBool.unset
        ? new bool[] {false, true}
        : new bool[] {throwForBase == nBool.setTrue};

      bool[] defaultEnSet = defaultEn == nBool.unset
        ? new bool[] {false, true}
        : new bool[] {defaultEn == nBool.setTrue};

      foreach (bool l in localeSet) {
        foreach (bool b in baseSet) {
          foreach (bool d in defaultEnSet) {
            // Skip if both "throwForLocale" and "defaultEn"
            if (!(l && d)) {
              bList.Add(getSample(l, b, d));
            }
          }
        }
      }

      return bList.ToArray();
    }

    private LocalizationMap getSample(bool throwForLocale, bool throwForBase, bool defaultEn) {
      LocalizationMap.MissingLocalesConfiguration mlc;
      LocalizationMap tmap;

      if (throwForLocale) {
        mlc = LocalizationMap.MissingLocalesConfiguration.ThrowError;
      } else if (defaultEn) {
        mlc = LocalizationMap.MissingLocalesConfiguration.UseDefaultLocale;
      } else {
        mlc = LocalizationMap.MissingLocalesConfiguration.UseBaseString;
      }

      tmap = new LocalizationMap(mlc, throwForBase);

      initialize(tmap);

      return tmap;
    }

    private void initialize(LocalizationMap tmap) {
      tmap.addLocalization(
        PRESENT_1_BASE_STRING,
        new string[] {LOCALE_EN_US, LOCALE_ES_ES},
        new string[] {PRESENT_1_EN_US_STRING, PRESENT_1_ES_ES_STRING}
      );

      tmap.addLocalization(
        PRESENT_2_BASE_STRING,
        new string[] {LOCALE_EN_US, LOCALE_ES_ES},
        new string[] {PRESENT_2_EN_US_STRING, PRESENT_2_ES_ES_STRING}
      );

      tmap.addLocalization(
        EN_US_ONLY_BASE_STRING,
        new string[] {LOCALE_EN_US},
        new string[] {EN_US_STRING}
      );

      tmap.addLocalization(
        ES_ES_ONLY_BASE_STRING,
        new string[] {LOCALE_ES_ES},
        new string[] {ES_ES_STRING}
      );
    }

    [Test]
    public void When_MissingLocaleAndRequestingError_Expect_AnError() {
      LocalizationMap[] tmaps = getSamples(nBool.setTrue, nBool.unset, nBool.unset);

      System.Exception ex;

      Assert.AreEqual(2, tmaps.Length);

      foreach (LocalizationMap tmap in tmaps) {
        ex = null;

        try {
          tmap.translate(PRESENT_1_BASE_STRING, LOCALE_ABSENT);
        } catch (System.Exception exception) {
          ex = exception;
        }

        Assert.AreNotEqual(ex, null);
        StringAssert.StartsWith("Missing localization for locale ", ex.Message);
      }
    }

    [Test]
    public void When_MissingLocaleAndRequestingError_Expect_NoError() {
      LocalizationMap[] tmaps = getSamples(nBool.setFalse, nBool.unset, nBool.unset);

      System.Exception ex;

      Assert.AreEqual(4, tmaps.Length);

      foreach (LocalizationMap tmap in tmaps) {
        ex = null;

        try {
          tmap.translate(PRESENT_1_BASE_STRING, LOCALE_ABSENT);
        } catch (System.Exception exception) {
          ex = exception;
        }

        Assert.AreEqual(ex, null);
      }
    }

    [Test]
    public void When_MissingBaseStringAndRequestingError_Expect_AnError() {
      LocalizationMap[] tmaps = getSamples(nBool.unset, nBool.setTrue, nBool.unset);

      System.Exception ex;

      Assert.AreEqual(3, tmaps.Length);

      foreach (LocalizationMap tmap in tmaps) {
        ex = null;

        try {
          tmap.translate(ABSENT_BASE_STRING, LOCALE_EN_US);
        } catch (System.Exception exception) {
          ex = exception;
        }

        Assert.AreNotEqual(ex, null);
        StringAssert.StartsWith("Missing base string ", ex.Message);
      }
    }

    [Test]
    public void When_MissingBaseStringAndRequestingError_Expect_NoError() {
      LocalizationMap[] tmaps = getSamples(nBool.unset, nBool.setFalse, nBool.unset);

      System.Exception ex;

      Assert.AreEqual(3, tmaps.Length);

      foreach (LocalizationMap tmap in tmaps) {
        ex = null;

        try {
          tmap.translate(ABSENT_BASE_STRING, LOCALE_EN_US);
        } catch (System.Exception exception) {
          ex = exception;
        }

        Assert.AreEqual(ex, null);
      }
    }

    [Test]
    public void When_RequestingEnUS_Expect_EnUSString() {
      LocalizationMap[] tmaps = getSamples(nBool.unset, nBool.unset, nBool.unset);

      Assert.AreEqual(6, tmaps.Length);

      foreach (LocalizationMap tmap in tmaps) {
        Assert.AreEqual(PRESENT_1_EN_US_STRING, tmap.translate(PRESENT_1_BASE_STRING, LOCALE_EN_US));
      }
    }

    [Test]
    public void When_RequestingEnUSDefaultAndNoErrors_Expect_EnUSString() {
      LocalizationMap tmap = getSample(false, false, true);

      Assert.AreEqual(EN_US_STRING, tmap.translate(EN_US_ONLY_BASE_STRING, LOCALE_EN_US));
      Assert.AreEqual(EN_US_STRING, tmap.translate(EN_US_ONLY_BASE_STRING, LOCALE_ES_ES));
    }

    [Test]
    public void When_RequestingNoEnUSDefaultAndNoErrors_Expect_BaseString() {
      LocalizationMap tmap = getSample(false, false, false);

      Assert.AreEqual(EN_US_STRING, tmap.translate(EN_US_ONLY_BASE_STRING, LOCALE_EN_US));
      Assert.AreEqual(EN_US_ONLY_BASE_STRING, tmap.translate(EN_US_ONLY_BASE_STRING, LOCALE_ES_ES));
    }
  }
}

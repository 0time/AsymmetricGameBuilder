using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using me.zti.ui;

namespace me.zti.files {
  public class CsvTests {
    [Test]
    public void When_OneLine_Expect_NoCalls() {
      string str = "Base String,en-US";
      int callCount = 0;

      Csv.DeserializeWithHeader(new StringReader(str), (Dictionary<string, string> keyedRow) => {
        ++callCount;
      });

      Assert.AreEqual(0, callCount);
    }

    [Test]
    public void When_TwoLines_Expect_OneCall() {
      string str = "Base String,en-US,es-EC\nBASE_STRING,English-UnitedStates,Español-Ecuador";
      int callCount = 0;

      Csv.DeserializeWithHeader(new StringReader(str), (Dictionary<string, string> keyedRow) => {
        ++callCount;

        Assert.AreEqual(keyedRow["Base String"], "BASE_STRING");
        Assert.AreEqual(keyedRow["en-US"], "English-UnitedStates");
        Assert.AreEqual(keyedRow["es-EC"], "Español-Ecuador");
      });

      Assert.AreEqual(1, callCount);
    }

    [Test]
    public void When_ThreeLines_Expect_TwoCalls() {
      string str = "A\n1\n2";
      int callCount = 0;

      Csv.DeserializeWithHeader(new StringReader(str), (Dictionary<string, string> keyedRow) => {
        ++callCount;

        Assert.AreEqual(keyedRow["A"], ((char)('0' + callCount)).ToString());
      });

      Assert.AreEqual(2, callCount);
    }

    [Test]
    public void When_CommasWrappedInQuotes_Expect_CommasIncludedQuotesExcluded() {
      string str = "\"A,Field\"\n\"A,Value\"";
      int callCount = 0;

      Csv.DeserializeWithHeader(new StringReader(str), (Dictionary<string, string> keyedRow) => {
        ++callCount;

        Assert.AreEqual(keyedRow["A,Field"], "A,Value");
      });

      Assert.AreEqual(1, callCount);
    }

    [Test]
    public void When_EscapedQuotes_Expect_EscapedQuotesIncluded() {
      string str = "\"\"\"AQuotedField\"\"\"\n\"\"\"AQuotedValue\"\"\"";
      int callCount = 0;

      Csv.DeserializeWithHeader(new StringReader(str), (Dictionary<string, string> keyedRow) => {
        ++callCount;

        Assert.AreEqual(keyedRow["\"AQuotedField\""], "\"AQuotedValue\"");
      });

      Assert.AreEqual(1, callCount);
    }

    [Test]
    public void When_EscapedQuotesInMiddle_Expect_EscapedQuotesIncluded() {
      string str = "\"AFieldWithA\"\"Quote\"\n\"AValueWithA\"\"Quote\"";
      int callCount = 0;

      Csv.DeserializeWithHeader(new StringReader(str), (Dictionary<string, string> keyedRow) => {
        ++callCount;

        Assert.AreEqual(keyedRow["AFieldWithA\"Quote"], "AValueWithA\"Quote");
      });

      Assert.AreEqual(1, callCount);
    }
  }
}

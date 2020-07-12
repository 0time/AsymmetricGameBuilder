using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Csv {
  public delegate void RowProcessor(Dictionary<string, string> keyedRow);
  // "(?:^|,)(\"(?:[^\"])*\"|[^,]*)"
  private static Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);


  public static List<Dictionary<string, string>> DeserializeWithHeader(string path) {
    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

    DeserializeWithHeader(path, (row) => {
      rows.Add(row);
    });

    return rows;
  }

  public static void DeserializeWithHeader(string path, RowProcessor rowProcessor) {
    DeserializeWithHeader(new StreamReader(path), rowProcessor);
  }

  public static void DeserializeWithHeader(TextReader textReader, RowProcessor rowProcessor) {
    Dictionary<string, string> keyedRow = new Dictionary<string, string>();
    string[] header = new string[] {};
    int index;
    bool initialized = false;
    int maxLength = 0;

    for (List<string> rawRow = ParseRow(textReader, ',', '"', maxLength); rawRow != null; rawRow = ParseRow(textReader, ',', '"', maxLength)) {
      index = 0;

      if (!initialized) {
        header = rawRow.ToArray();
      } else {
        keyedRow = new Dictionary<string, string>();

        foreach (string match in rawRow) {
          keyedRow.Add(header[index], match);

          ++index;
        }
      }


      if (initialized) {
        rowProcessor(keyedRow);
      } else {
        initialized = true;
      }

      maxLength = (maxLength > rawRow.Count ? maxLength : rawRow.Count);
    }
  }

  public static List<string> ParseRow(TextReader textReader, char delimiter, char qualifier, int guessLength = -1) {
    bool inQuote = false;
    List<string> record = guessLength > 0 ? new List<string>(guessLength) : new List<string>();
    StringBuilder stringBuilder = new StringBuilder();

    while (textReader.Peek() != -1) {
      char readChar = (char) textReader.Read();

      if (readChar == '\n' || (readChar == '\r' && (char) textReader.Peek() == '\n')) {
        if (readChar == '\r') {
          textReader.Read();
        }

        if (inQuote) {
          if (readChar == '\r') {
            stringBuilder.Append(readChar);
          }

          stringBuilder.Append('\n');
        } else {
          if (record.Count > 0 || stringBuilder.Length > 0) {
            record.Add(stringBuilder.ToString());
            stringBuilder.Clear();
          }

          return record;
        }
      } else if (stringBuilder.Length == 0 && !inQuote) {
        if (readChar == qualifier) {
          inQuote = true;
        } else if (readChar == delimiter) {
          record.Add(stringBuilder.ToString());
          stringBuilder.Clear();
        } else if (char.IsWhiteSpace(readChar)) {
          // Ignore leading whitespace
        } else {
          stringBuilder.Append(readChar);
        }
      } else if (readChar == delimiter) {
        if (inQuote) {
          stringBuilder.Append(delimiter);
        } else {
          record.Add(stringBuilder.ToString());
          stringBuilder.Clear();
        }
      } else if (readChar == qualifier) {
        if (inQuote) {
          if ((char) textReader.Peek() == qualifier) {
            textReader.Read();
            stringBuilder.Append(qualifier);
          } else {
            inQuote = false;
          }
        } else {
          stringBuilder.Append(readChar);
        }
      } else {
        stringBuilder.Append(readChar);
      }
    }

    if (record.Count > 0 || stringBuilder.Length > 0) {
      record.Add(stringBuilder.ToString());
      stringBuilder.Clear();
    }

    return record.Count > 0 ? record : null;
  }
}

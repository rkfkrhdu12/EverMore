using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVParser
{
    public static List<string> Read(string fileName)
    {
        var list = new List<string>();

        TextAsset data = Resources.Load(fileName) as TextAsset;

        if (null == data) { return null; }

        var lines = Regex.Split(data.text, "\r\n");

        if (lines.Length <= 1) return null;

        for (int i = 1; i < lines.Length; ++i)
        {
            if (lines[i].Equals(string.Empty)) continue;

            list.Add(lines[i]);
        }

        return list;
    }
}

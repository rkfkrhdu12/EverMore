using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class CSVParser
{
    public static List<string> Read(string file)
    {
        var list = new List<string>();
        TextAsset data = Resources.Load(file) as TextAsset;
        if (null == data) return list;

        var lines = Regex.Split(data.text, "\r\n");

        if (lines.Length <= 1) return list;

        for (int i = 1; i < lines.Length; ++i) 
        {
            if ("" == lines[i]) continue;

            list.Add(lines[i]);
        }
        
        return list;
    }

}

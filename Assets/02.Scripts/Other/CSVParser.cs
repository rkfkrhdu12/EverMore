using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class CSVParser
{
    //IEnumerator LoadItemList(UnityAction<TextAsset> ppap)
    //{
    //    Addressables.LoadAssetAsync<TextAsset>("ItemList").Completed += op =>
    //    {
    //         ppap(op.Result);
    //    };

    //    yield return null;
    //  }


    //private async void da()
    //{
    //    await Addressables.LoadAssetAsync<TextAsset>("ItemList");

    //}

    public async static void LoadList(string fileName, UnityAction<TextAsset> ppap)
    {
        var returnVal = await Addressables.LoadAssetAsync<TextAsset>(fileName).Task;

        Debug.Log(returnVal);

        ppap(returnVal);
    }

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

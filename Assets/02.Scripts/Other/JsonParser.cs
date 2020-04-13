using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    public static string Write(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public static T Read<T>(string fileName)
    {
        return JsonUtility.FromJson<T>(fileName);
    }
}

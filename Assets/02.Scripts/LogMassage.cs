using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMessage
{
    public static void Log(string str)
    {
        #if UNITY_EDITOR
        Debug.Log(str);
        #endif
    }

    public static void LogError(string str)
    {
        #if UNITY_EDITOR
        Debug.LogError(str);
        #endif
    }
}

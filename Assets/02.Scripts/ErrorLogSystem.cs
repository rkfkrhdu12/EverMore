using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorLogSystem
{
    public static void Log(string str)
    {
#if UNITY_EDITOR
        Debug.Log(str);
#endif
    }
}

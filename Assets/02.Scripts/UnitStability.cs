using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStability
{
    /// <summary>
    /// 안정도 당 공격속도 리턴(-1 리턴시 오류)
    /// </summary>
    /// <param name="stability"> 안정도 </param>
    /// <returns> 안정도 당 공격속도 </returns>
    public static float GetStabilityPerAttackSpeed(int stability)
    {
        if(0 < stability || 100 > stability) { return -1; }

        float returnVal = 0;

        int digitOfTen = (int)((float)stability / 10);

        returnVal = _stabilityPerAttackSpeeds[digitOfTen];

        int digitOfOne = stability % 10;
        float SPAInterval = _stabilityPerAttackSpeeds[digitOfTen + 1] - _stabilityPerAttackSpeeds[digitOfTen];

        returnVal += SPAInterval * digitOfOne;

        return returnVal;
    }

    /// <summary>
    /// 안정도 당 이동속도 리턴(-1 리턴시 오류)
    /// </summary>
    /// <param name="stability"> 안정도 </param>
    /// <returns> 안정도 당 이동속도 </returns>
    public static float GetStabilityPerMoveSpeed(int stability)
    {
        if (0 > stability || 100 < stability) { return -1; }

        float returnVal = 0;

        int digitOfTen = (int)((float)stability / 10);

        returnVal = _stabilityPerMoveSpeeds[digitOfTen];

        float digitOfOne = (stability % 10.0f) / 10.0f;
        float SPAInterval = _stabilityPerMoveSpeeds[digitOfTen + 1] -
                            _stabilityPerMoveSpeeds[digitOfTen];

        returnVal += SPAInterval * digitOfOne;

        Debug.Log(_stabilityPerMoveSpeeds[digitOfTen] + "   " + SPAInterval + "   " + returnVal);

        return returnVal;
    }

    #region Variable

    static float[] _stabilityPerAttackSpeeds = new float[11];
    static float[] _stabilityPerMoveSpeeds = new float[11];

    #endregion

    #region Private Fuction

    private void Awake()
    {
        Init();
    }

    // Test
    public static void Init()
    {
        int i = 0;
        _stabilityPerAttackSpeeds[i++] = 0.5f;
        _stabilityPerAttackSpeeds[i++] = 0.6f;
        _stabilityPerAttackSpeeds[i++] = 0.7f;
        _stabilityPerAttackSpeeds[i++] = 0.8f;
        _stabilityPerAttackSpeeds[i++] = 1.0f;
        _stabilityPerAttackSpeeds[i++] = 1.2f;
        _stabilityPerAttackSpeeds[i++] = 1.4f;
        _stabilityPerAttackSpeeds[i++] = 1.6f;
        _stabilityPerAttackSpeeds[i++] = 1.8f;
        _stabilityPerAttackSpeeds[i++] = 1.9f;
        _stabilityPerAttackSpeeds[i++] = 2.0f;

        i = 0;
        _stabilityPerMoveSpeeds[i++] = 0.1f;
        _stabilityPerMoveSpeeds[i++] = 0.4f;
        _stabilityPerMoveSpeeds[i++] = 0.7f;
        _stabilityPerMoveSpeeds[i++] = 1.0f;
        _stabilityPerMoveSpeeds[i++] = 1.2f;
        _stabilityPerMoveSpeeds[i++] = 1.25f;
        _stabilityPerMoveSpeeds[i++] = 1.3f;
        _stabilityPerMoveSpeeds[i++] = 1.4f;
        _stabilityPerMoveSpeeds[i++] = 1.5f;
        _stabilityPerMoveSpeeds[i++] = 2.0f;
        _stabilityPerMoveSpeeds[i++] = 2.5f;
    }

    #endregion
}

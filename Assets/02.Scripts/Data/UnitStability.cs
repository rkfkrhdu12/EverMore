using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnitStability
{
    /// <summary>
    /// 안정도 당 공격속도 리턴(-1 리턴시 오류)
    /// </summary>
    /// <param name="stability"> 안정도 </param>
    /// <returns> 안정도 당 공격속도 </returns>
    public static float GetStabilityPerAttackSpeed(int stability)
    {
        if (_stabilityPerAttackSpeeds == null) { Init(); }
        if (stability < 0 || stability > 100)
            return -1;

        int digitOfTen = (int) ((float) stability / 10);

        var returnVal = _stabilityPerAttackSpeeds[digitOfTen];

        if (stability < 100)
        {
            int digitOfOne = stability % 10;
            float SPAInterval = _stabilityPerAttackSpeeds[digitOfTen + 1] - _stabilityPerAttackSpeeds[digitOfTen];
            returnVal += SPAInterval * (digitOfOne / 10);
        }

        return returnVal;
    }

    /// <summary>
    /// 안정도 당 이동속도 리턴(-1 리턴시 오류)
    /// </summary>
    /// <param name="stability"> 안정도 </param>
    /// <returns> 안정도 당 이동속도 </returns>
    public static float GetStabilityPerMoveSpeed(int stability)
    {
        if(_stabilityPerMoveSpeeds == null) { Init(); }
        if (stability < 0 || stability > 100)
            return -1;

        int digitOfTen = (int) ((float) stability / 10);

        var returnVal = _stabilityPerMoveSpeeds[digitOfTen];

        if (stability < 100)
        {
            float digitOfOne = stability % 10.0f / 10.0f;
            float SPAInterval = _stabilityPerMoveSpeeds[digitOfTen + 1] -
                                _stabilityPerMoveSpeeds[digitOfTen];

            returnVal += SPAInterval * digitOfOne;
        }
        return returnVal;
    }

    #region Variable

    private static float[] _stabilityPerAttackSpeeds;
    private static float[] _stabilityPerMoveSpeeds;

    #endregion

    #region Private Fuction

    private static void Init()
    {
        //int i = 0;
        //_stabilityPerAttackSpeeds[i++] = 0.5f;
        //_stabilityPerAttackSpeeds[i++] = 0.6f;
        //_stabilityPerAttackSpeeds[i++] = 0.7f;
        //_stabilityPerAttackSpeeds[i++] = 0.8f;
        //_stabilityPerAttackSpeeds[i++] = 1.0f;
        //_stabilityPerAttackSpeeds[i++] = 1.2f;
        //_stabilityPerAttackSpeeds[i++] = 1.4f;
        //_stabilityPerAttackSpeeds[i++] = 1.6f;
        //_stabilityPerAttackSpeeds[i++] = 1.8f;
        //_stabilityPerAttackSpeeds[i++] = 1.9f;
        //_stabilityPerAttackSpeeds[i++] = 2.0f;

        //i = 0;
        //_stabilityPerMoveSpeeds[i++] = 0.1f;
        //_stabilityPerMoveSpeeds[i++] = 0.4f;
        //_stabilityPerMoveSpeeds[i++] = 0.7f;
        //_stabilityPerMoveSpeeds[i++] = 1.0f;
        //_stabilityPerMoveSpeeds[i++] = 1.2f;
        //_stabilityPerMoveSpeeds[i++] = 1.25f;
        //_stabilityPerMoveSpeeds[i++] = 1.3f;
        //_stabilityPerMoveSpeeds[i++] = 1.4f;
        //_stabilityPerMoveSpeeds[i++] = 1.5f;
        //_stabilityPerMoveSpeeds[i++] = 2.0f;
        //_stabilityPerMoveSpeeds[i++] = 2.5f;

        List<string> stabilityList = CSVParser.Read("WeightBalance");

        _stabilityPerAttackSpeeds   = new float[11];
        _stabilityPerMoveSpeeds     = new float[11];

        var splitDatas = stabilityList[0].Split(',');
        for (int i = 0; i < 11; ++i) 
        {
            float.TryParse(splitDatas[i + 1], out _stabilityPerAttackSpeeds[i]);
        }

        splitDatas = stabilityList[1].Split(',');
        for (int i = 0; i < 11; ++i)
        {
            float.TryParse(splitDatas[i + 1], out _stabilityPerMoveSpeeds[i]);
        }
    }

    #endregion
}

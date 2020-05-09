using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

public class Team
{
    public string Name { set; get; } = "";

    public const int UnitCount = 6;

    private UnitStatus[] _units = new UnitStatus[UnitCount];
    public int Length { get { return _units.Length; } }

    public void Init()
    {
        for (int i = 0; i < Length; ++i)
        {
           _units[i].Init();
        }
    }

    public void Spawn(Vector3 spawnPoint, int unitNum)
    {
    }

    public ref UnitStatus GetUnit(int index)
    {
        return ref _units[index];
    }


}

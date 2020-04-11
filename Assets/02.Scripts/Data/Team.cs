﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public const int UnitCount = 6;

    //public UnitData[] _units = new UnitData[UnitCount];
    public List<UnitData> _units = new List<UnitData>();

    void AddUnit(UnitData unitData)
    {
        if (UnitCount <= _units.Count) { return; }

        _units.Add(unitData);
    }

    //테스트 용도 함수 입니다.
    public void InitTest()
    {
        {
            UnitData unit = new UnitData();
            unit.Init();
            unit.Equip(1);
            unit.Equip(2);
            AddUnit(unit);
        }


        {
            UnitData unit = new UnitData();
            unit.Init();
            unit.Equip(3);
            unit.Equip(4);
            AddUnit(unit);
        }
        {
            UnitData unit = new UnitData();
            unit.Init();
            unit.Equip(5);
            unit.Equip(6);
        }
        {
            UnitData unit = new UnitData();
            unit.Init();
            unit.Equip(7);
            unit.Equip(8);
        }
        {
            UnitData unit = new UnitData();
            unit.Init();
            unit.Equip(13);
            unit.Equip(9);
        }
        {
            UnitData unit = new UnitData();
            unit.Init();
            unit.Equip(14);
            unit.Equip(9);
        }
    }
}

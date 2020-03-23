﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public static readonly int UnitCount = 6;

    public Unit[] _units = new Unit[UnitCount];

    public void InitTest()
    {
        int i = 0;
        _units[i] = new Unit();
        _units[i].Init();
        _units[i].Equip(1);
        _units[i++].Equip(2);

        _units[i] = new Unit();
        _units[i].Init();
        _units[i].Equip(3);
        _units[i++].Equip(4);

        _units[i] = new Unit();
        _units[i].Init();
        _units[i].Equip(5);
        _units[i++].Equip(6);

        _units[i] = new Unit();
        _units[i].Init();
        _units[i].Equip(7);
        _units[i++].Equip(8);

        _units[i] = new Unit();
        _units[i].Init();
        _units[i].Equip(13);
        _units[i++].Equip(9);

        _units[i] = new Unit();
        _units[i].Init();
        _units[i].Equip(14);
        _units[i].Equip(9);
    }
}

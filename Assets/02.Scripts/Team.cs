using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public Unit[] _units = new Unit[5];

    public void InitTest()
    {
        int i = 0;
        _units[i] = new Unit();
        _units[i].Equip(1);
        _units[i].Equip(2);
        _units[i++].Init();

        _units[i] = new Unit();
        _units[i].Equip(3);
        _units[i].Equip(4);
        _units[i++].Init();

        _units[i] = new Unit();
        _units[i].Equip(5);
        _units[i].Equip(6);
        _units[i++].Init();

        _units[i] = new Unit();
        _units[i].Equip(7);
        _units[i].Equip(8);
        _units[i++].Init();

        _units[i] = new Unit();
        _units[i].Equip(11);
        _units[i].Equip(12);
        _units[i++].Init();
    }
}

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

    public UnitStatus GetUnit(int index)
    {
        if(_units[index]._equipedItems == null) { _units[index].Init(); }

        return _units[index];
    }

    public void SetEquipedItems(int index, int[] items)
    {
        if(_units[index]._equipedItems == null) { _units[index].Init(); }

        _units[index]._equipedItems = items;
    }
    
    public void UpdateItems()
    {
        for (int i = 0; i < Length; ++i)
        {
            _units[i].UpdateItems();
        }
    }
}

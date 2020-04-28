using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

public class Team
{
    [SerializeField]
    GameObject _unitPrefab = null;

    public string Name { set; get; } = "";

    public const int UnitCount = 6;

    private UnitController[] _units = new UnitController[UnitCount];
    public int Length { get { return _units.Length; } }

    //public void AddUnit(UnitData unitData)
    //{
    //    //유닛 개수가 UnitCount보다 이상이면 : return
    //    if (_units.Length >= UnitCount)
    //        return;
    //    if(null == unitData) { return; }

    //    //추가
    //    _units.Add(unitData);
    //}

    public void Spawn(Vector3 spawnPoint, int unitNum)
    {
        if (0 > unitNum || unitNum >= _units.Length) { return; }
        if (null == _units[unitNum]) { return; }

        //Object.Instantiate()
    }


    public UnitController GetUnit(int index)
    {
        if (index >= _units.Length || index < 0) { Debug.Log(" Team : UpdateUnit index Error"); return null; }
        
        // null == 0
        if (null == _units[index])
        { // 유닛이 없을때
            _units[index] = new UnitController();

            _units[index].Init();
        }
        else
        { // 유닛이 있을때

        }

        return _units[index];
    }

    public Texture GetUnitTexture(int index)
    {
        if((index <= 0 && index > _units.Length) || null == _units[index]) { return null; }

        ref stringTexture2D st = ref Manager.Get<GameManager>().st;

        if (!st.ContainsKey(_units[index]._3DModelToTextureName)) { return null; }

        Texture returnTexture = st[_units[index]._3DModelToTextureName];

        return returnTexture;
    }

    //테스트 용도 함수 입니다.
    //public void InitTest()
    //{
    ////    UnitData가 FieldObject를 상속받고 있고, FieldObject는 Mono비헤비어를 상속받고 있어서,
    ////다음과 같이 new로 객체 생성 X

    // {
    //        UnitData unit = new UnitData();
    //        unit.Init();
    //        unit.Equip(1);
    //        unit.Equip(2);
    //        AddUnit(unit);
    //    }
    //    {
    //        UnitData unit = new UnitData();
    //        unit.Init();
    //        unit.Equip(3);
    //        unit.Equip(4);
    //        AddUnit(unit);
    //    }
    //    {
    //        UnitData unit = new UnitData();
    //        unit.Init();
    //        unit.Equip(5);
    //        unit.Equip(6);
    //    }
    //    {
    //        UnitData unit = new UnitData();
    //        unit.Init();
    //        unit.Equip(7);
    //        unit.Equip(8);
    //    }
    //    {
    //        UnitData unit = new UnitData();
    //        unit.Init();
    //        unit.Equip(13);
    //        unit.Equip(9);
    //    }
    //    {
    //        UnitData unit = new UnitData();
    //        unit.Init();
    //        unit.Equip(14);
    //        unit.Equip(9);
    //    }
    //}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public enum eCodeType
{
    HELMET,
    BODYARMOUR,
    WEAPON,
}

public class ItemList
{
    /// <summary>
    /// 아이템코드(int)를 넣으면 아이템정보(Item)를 return
    /// </summary>
    /// <param name="index"> 아이템 코드 </param>
    /// <returns> 아이템 </returns>
    public Item ItemSearch(int index)
    {
        if (!_itemList.ContainsKey(index) || index < 0) { return null; }

        return _itemList[index];        
    }

    /// <summary>
    /// 원하는 아이템타입(eCodeType)과 번호(int)를 넣으면 아이템코드(int) return (추천)
    /// </summary>
    /// <param name="codeType"> eCodeType 의 형태로 된 enum값 </param>
    /// <param name="index"> 원하는 아이템의 번호 </param>
    /// <returns> 아이템코드 </returns>
    public int CodeSearch(eCodeType codeType, int index)
    {
        int tpye = (int)codeType;

        if (index >= _codeList[tpye].Count) return -1;

        return _codeList[(int)codeType][index];
    }

    // Test
    public int ItemCount(eCodeType codeType)
    {
        return _codeList[(int)codeType].Count;
    }

    /// <summary>
    /// 원하는 아이템타입(eCodeType)과 아이템이름(string)를 넣으면 아이템코드(int) return  (비추천)
    /// </summary>
    /// <param name="codeType"> eCodeType 의 형태로 된 enum값 </param>
    /// <param name="name"> 원하는 아이템의 Name </param>
    /// <returns> 아이템코드 </returns>
    public int CodeSearch(eCodeType codeType, string name)
    {
        int tpye = (int)codeType;
        
        for (int i = 0; i < _codeList[tpye].Count; ++i)
        {
            int code = CodeSearch(codeType, i);

            if(string.Equals(ItemSearch(code).Name,name))
            {
                return code;
            }
        }

        return -1;
    }

    /// <summary>
    /// Set ItemList        | GameSystem 에서 해주고 있음 
    /// </summary>
    public void Init()
    {
        if (_isInit) return;
        _isInit = true;

        _codeList[(int)eCodeType.HELMET] = new Dictionary<int, int>();
        _codeList[(int)eCodeType.BODYARMOUR] = new Dictionary<int, int>();
        _codeList[(int)eCodeType.WEAPON] = new Dictionary<int, int>();

        // 길이가 유동적이고 검색을 하는것이 아니므로 속도가 크게 상관없어 List를 이용
        List<string> itemDatas = CSVParser.Read("ItemList");

        for (int i = 0; i < itemDatas.Count; ++i)
        {
            string[] splitDatas = itemDatas[i].Split(',');

            AddList(splitDatas);
        }
    }

    #region Variable

    // 아이템은 빨리 자주 찾으며, 키를 통해 찾을것이므로 map(Dictionary)을 이용
    Dictionary<int, Item> _itemList = new Dictionary<int, Item>();

    // 종류를 번호순으로 정렬하여 저장, ex) _codeList[(int)eCodeType.Weapon][0] == Index와 상관없이 리스트중 첫번째 무기 리턴
    Dictionary<int, int>[] _codeList = new Dictionary<int, int>[3];

    bool _isInit = false;
    #endregion

    #region Private Fuction
    
    void AddList(string[] data)
    {
        Item i = null;

        switch (data[1])
        {
            // Weapon
            case ItemTypeList.OneHandSword: { i = new OneHandSword(); break; }
            case ItemTypeList.Shield: { i = new Shield(); break; }
            case ItemTypeList.Dagger: { i = new Dagger(); break; }

            // Armour
            case ItemTypeList.Helmet: { i = new Helmet(); break; }
            case ItemTypeList.BodyArmour: { i = new BodyArmour(); break; }
        }

        if (null == i) { return; }
        if (eItemType.NONE == i.Type) { return; }

        int index = int.Parse(data[0]);

        switch (i.Type)
        {
            case eItemType.NONE: Debug.LogError("Item Error" + i.Name); break;
            case eItemType.HELMET: _codeList[(int)eCodeType.HELMET]             .Add(_codeList[(int)eCodeType.HELMET].Count, index);        break;
            case eItemType.BODYARMOUR: _codeList[(int)eCodeType.BODYARMOUR]     .Add(_codeList[(int)eCodeType.BODYARMOUR].Count, index);    break;
            default: _codeList[(int)eCodeType.WEAPON]                           .Add(_codeList[(int)eCodeType.WEAPON].Count, index);        break;
        }

        GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/03.Prefabs/Item/" + data[0] + ".prefab", typeof(GameObject));

        // '/' 가 있음(왼쪽(방어구류),오른쪽(무기류)) 없으면 모든아이템 해당
        i.Init(data[2],                 // Name
            int.Parse(data[3]),         // Cost
            float.Parse(data[4]),       // CoolTime
            float.Parse(data[6]),       // Defense / Damage
            float.Parse(data[7]),       // Health  / Range
            int.Parse(data[8]),
            obj);        // Weight
        _itemList.Add(index, i);
    }

    #endregion
}

// 파싱한 데이터는 string 이므로 const string 으로 비교하여 판단
struct ItemTypeList
{
    public const string Helmet          = "모자";
    public const string BodyArmour      = "갑옷";
    public const string OneHandSword    = "한손검";
    public const string Shield          = "방패";
    public const string Dagger          = "단검";
};

public enum eItemType
{
    NONE,

    // Armour
    HELMET,
    BODYARMOUR,

    // Weapon
    ONEHANDSWORD,
    SHIELD,
    DAGGER,
}

public class Item
{
    protected string _name;                     public string Name { get => _name; }
    protected eItemType _type = eItemType.NONE; public eItemType Type { get => _type; }
    protected int _cost;                        public int Cost { get => _cost; }
    protected float _coolTime;                  //public float CoolTime { get => _coolTime; }
    protected int _weight;
    protected UnityAction _ability;
    protected GameObject _object;                  public GameObject Object { get => _object; }

    public virtual void Init(string name, int cost, float coolTime, float data1, float data2, int weight, GameObject obj) { }

    protected void Init(string name,int cost,float coolTime, int weight, GameObject obj)
    {
        _name = name;
        _cost = cost;
        _coolTime = coolTime;
        _weight = weight;
        _object = obj;
    }
}

public class Weapon : Item
{
    protected float _range;
    protected float _damage;

    public override void Init(string name, int cost, float coolTime, float damage, float range, int weight, GameObject obj)
    {
        Init(name, cost, coolTime, weight, obj);

        _range = range;
        _damage = damage;
    }
}

public class Armour : Item
{
    protected float _defense;
    protected float _health;

    public override void Init(string name, int cost, float coolTime, float defense, float health, int weight, GameObject obj)
    {
        Init(name, cost, coolTime, weight, obj);

        _defense = defense;
        _health = health;
    }
}

#region Weapon

public class OneHandSword : Weapon
{
    public OneHandSword() { _type  = eItemType.ONEHANDSWORD; }
}

public class Shield : Weapon
{
    public Shield() { _type = eItemType.SHIELD; }

    protected float _defense { get { return _damage; } set { _damage = value; } }
}

public class Dagger : Weapon
{
    public Dagger() { _type = eItemType.DAGGER; }
}
#endregion

#region Armour

public class Helmet : Armour
{
    public Helmet() { _type = eItemType.HELMET; }
}

public class BodyArmour : Armour
{
    public BodyArmour() { _type = eItemType.BODYARMOUR; }
}

#endregion
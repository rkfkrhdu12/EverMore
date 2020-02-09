using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct WeaponList
{
    public const string OneHandSword = "한손검";
    public const string Shield = "방패";
};

public class ItemList : MonoBehaviour
{
    // 아이템은 빨리 자주 찾으며, 키를 통해 찾을것이므로 map(Dictionary)을 이용
    Dictionary<int, Weapon> _weaponList = new Dictionary<int, Weapon>();

    Dictionary<int, Armour> _armourList = new Dictionary<int, Armour>();

    void Start()
    {
        // 길이가 유동적이고 검색을 하는것이 아니므로 속도가 크게 상관없어 List를 이용
        List<string> data = CSVParser.Read("TestWeapon");

        for (int i = 0; i < data.Count; ++i)
        {
            string[] splitData = data[i].Split(',');

            WeaponListLoad(splitData);
        }

    }
    
    void WeaponListLoad(string[] data)
    {
        switch(data[1])
        {
            case WeaponList.OneHandSword:
                Weapon w = new OneHandSword();
                w.Init(eWeaponType.ONEHANDSWORD,
                                  data[2],
                    float.  Parse(data[3]),
                    float.  Parse(data[4]),
                    int.    Parse(data[5]),
                    int.    Parse(data[6]),           
                    float.  Parse(data[7]));
                _weaponList.Add(int.Parse(data[0]), w);
                break;
            case WeaponList.Shield:

                break;
        }
    }
    
}


public class Item
{
    public string _name;
    public int _cost;
    public float _coolTime;
    public UnityAction _ability;
}

#region Weapon

public enum eWeaponType
{
    ONEHANDSWORD,
    SHIELD,
}
public class Weapon : Item
{
    public eWeaponType _type;
    public float _range;
    public float _damage;
    public int _weight;

    public virtual void Init(eWeaponType type, string name, float range, float damage, int weight, int cost, float coolTime)
    {
        _type = type;
        _name = name;
        _range = range;
        _damage = damage;
        _weight = weight;
        _cost = cost;
        _coolTime = coolTime;
    }
}

public class OneHandSword : Weapon
{
    public override void Init(eWeaponType type, string name, float range, float damage, int weight, int cost, float coolTime)
    {
        _type = type;
        _name = name;
        _range = range;
        _damage = damage;
        _weight = weight;
        _cost = cost;
        _coolTime = coolTime;
    }
}
public class Shield : Weapon
{
    public float _defense;
    public new const float _damage = 0;
}
#endregion

#region Armour

public enum eArmourType
{
    HELMET,
    BODYARMOUR,
}
public class Armour : Item
{
    public float _defense;
    public float _health;
}

public class Helmet : Armour
{

}
public class BodyArmour : Armour
{

}

#endregion

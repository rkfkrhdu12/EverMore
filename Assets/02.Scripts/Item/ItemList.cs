using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

using GameItem;


// 파싱한 데이터는 string 이므로 const string 으로 비교하여 판단
internal struct ItemTypeList
{
    public const string Helmet = "모자";
    public const string BodyArmour = "갑옷";
    public const string OneHandSword = "한손검";
    public const string Shield = "방패";
    public const string Dagger = "단검";
}

public class ItemList
{
    #region Variable

    // 아이템은 빨리 자주 찾으며, 키를 통해 찾을것이므로 map(Dictionary)을 이용
    private Dictionary<int, Item> _itemList = new Dictionary<int, Item>();

    // 종류를 번호순으로 정렬하여 저장, ex) _codeList[(int)eCodeType.Weapon][0] == Index와 상관없이 리스트중 첫번째 무기 리턴
    private Dictionary<int, int>[] _codeList = new Dictionary<int, int>[3];

    private bool _isInit;

    #endregion

    #region Private Fuction

    private void AddList(IReadOnlyList<string> data)
    {
        if(string.IsNullOrEmpty(data[0])) { return; }

        Item i = null;

        switch (data[1])
        {
            // Weapon
            case ItemTypeList.OneHandSword:
            {
                i = new OneHandSword();
                break;
            }
            case ItemTypeList.Shield:
            {
                i = new Shield();
                break;
            }
            case ItemTypeList.Dagger:
            {
                i = new Dagger();
                break;
            }

            // Armour
            case ItemTypeList.Helmet:
            {
                i = new Helmet();
                break;
            }
            case ItemTypeList.BodyArmour:
            {
                i = new BodyArmour();
                break;
            }
        }

        if (i == null)
            return;

        if (i.Type == eItemType.NONE)
            return;

        int index = int.Parse(data[0]);

        switch (i.Type)
        {
            case eItemType.NONE:
            Debug.LogError("Item Error" + i.Name);
            break;
            case eItemType.HELMET:
            _codeList[(int)eCodeType.HELMET].Add(_codeList[(int)eCodeType.HELMET].Count, index);
            break;
            case eItemType.BODYARMOUR:
            _codeList[(int)eCodeType.BODYARMOUR].Add(_codeList[(int)eCodeType.BODYARMOUR].Count, index);
            break;
            default:
            _codeList[(int)eCodeType.WEAPON].Add(_codeList[(int)eCodeType.WEAPON].Count, index);
            break;
        }

        //GameObject obj =
        //    (GameObject)AssetDatabase.LoadAssetAtPath("Assets/03.Prefabs/Item/" + data[0] + ".prefab",
        //        typeof(GameObject));

        // '/' 가 있음(왼쪽(방어구류),오른쪽(무기류)) 없으면 모든아이템 해당

        //i.Init(data[2], // Name
        //    int.Parse(data[3]), // Cost
        //    float.Parse(data[4]), // CoolTime
        //    float.Parse(data[6]), // Defense 
        //                          // Damage
        //    float.Parse(data[7]), // Health  
        //                          // Range
        //    int.Parse(data[8]),
        //    null); // Weight
        i.Init(data);
        _itemList.Add(index, i);
    }

    #endregion

    /// <summary>
    /// Input int(아이템코드) | Item(아이템정보) return
    /// </summary>
    /// <param name="index"> 아이템 코드 </param>
    /// <returns> 아이템 </returns>
    public Item ItemSearch(int index)
    {
        if (!_itemList.ContainsKey(index) || index < 0)
        {
            return null;
        }

        return _itemList[index];
    }

    /// <summary>
    /// Input eCodeType(아이템타입), int(타입순서) | int(아이템코드) return
    /// </summary>
    /// <param name="codeType"> eCodeType 의 형태로 된 enum값 </param>
    /// <param name="index"> 원하는 아이템의 번호 </param>
    /// <returns> 아이템코드 </returns>
    public int CodeSearch(eCodeType codeType, int index)
    {
        int tpye = (int)codeType;

        if (index >= _codeList[tpye].Count) return -1;

        return _codeList[tpye][index];
    }

    /// <summary>
    /// Input eCodeType, string | int(아이템코드) return
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

            if (string.Equals(ItemSearch(code).Name, name))
                return code;
        }

        return -1;
    }

    /// <summary>
    /// Initialize        | GameSystem 에서 해주고 있음 
    /// </summary>
    public void Init()
    {
        if (_isInit) return;
        _isInit = true;

        _codeList[(int)eCodeType.HELMET] = new Dictionary<int, int>();
        _codeList[(int)eCodeType.BODYARMOUR] = new Dictionary<int, int>();
        _codeList[(int)eCodeType.WEAPON] = new Dictionary<int, int>();

        // CSV Parser
        {
            // 길이가 유동적이고 검색을 하는것이 아니므로 속도가 크게 상관없어 List를 이용
            List<string> itemDatas = CSVParser.Read("ItemList");
            if(null == itemDatas) { return; }

            //itemDatas에서 ','으로 나뉘어진 것을 선택하여 가져온다는 Linq구문이다.
            foreach (var splitDatas in itemDatas.Select(t => t.Split(',')))
                AddList(splitDatas);
        }
    }

    // Test
    public int ItemCount(eCodeType codeType)
        => _codeList[(int)codeType].Count;

    private string[] _textureNames;

    private void InitTexture()
    {
        _textureNames = new string[16];


    }

    public string TextureName(int index)
    {
        if(System.Convert.ToUInt32(index) > _textureNames.Length) { return ""; }

        return _textureNames[index];
    }
}
namespace GameItem
{
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

    public enum eCodeType
    {
        HELMET,
        BODYARMOUR,
        WEAPON,
    }

    public delegate void ItemAbility();

    public class Item
    {
        protected string _name;                         public string Name => _name;

        protected eItemType _type = eItemType.NONE;     public eItemType Type => _type;

        protected int _cost;                            // public int Cost => _cost;

        protected float _coolTime;                      // public float CoolTime { get => _coolTime; }
        protected int _weight;
        protected ItemAbility _ability;                 public ItemAbility Ability => _ability;

        public virtual void Init(IReadOnlyList<string> datas)
        {
            if (null == datas[0]) { return; }

            _name = datas[2];
            int.TryParse(datas[3], out _cost);
            float.TryParse(datas[4], out _coolTime);
            int.TryParse(datas[10], out _weight);
        }

        public virtual void Equip(ref UnitStatus us)
        {
            us._cost        += _cost;
            us._coolTime    += _coolTime;
            us._weight      += _weight;
        }

        public virtual void UnEquip(ref UnitStatus us)
        {
            us._cost        -= _cost;
            us._coolTime    -= _coolTime;
            us._weight      -= _weight;
        }
    }

    public class Weapon : Item
    {
        protected float _range;
        protected float _damage;

        public override void Init(IReadOnlyList<string> datas)
        {
            base.Init(datas);

            float.TryParse(datas[9], out _range);
            float.TryParse(datas[7], out _damage);
        }

        public override void Equip(ref UnitStatus us)
        {
            base.Equip(ref us);

            us._attackRange += _range;
            us._attackDamage += _damage;
        }

        public override void UnEquip(ref UnitStatus us)
        {
            base.Equip(ref us);

            us._attackRange  -= _range;
            us._attackDamage -= _damage;
        }
    }

    public class Armour : Item
    {
        protected float _defense;
        protected float _health;

        public override void Init(IReadOnlyList<string> datas)
        {
            base.Init(datas);

            float.TryParse(datas[6], out _defense);
            float.TryParse(datas[8], out _health);
        }

        public override void Equip(ref UnitStatus us)
        {
            base.Equip(ref us);

            us._defensivePower  += _defense;
            us._maxhealth       += _health;
            us._curhealth       += _health;
        }

        public override void UnEquip(ref UnitStatus us)
        {
            base.Equip(ref us);

            us._defensivePower  -= _defense;
            us._maxhealth       -= _health;
            us._curhealth       -= _health;
        }
    }

    #region Weapon

    public class OneHandSword : Weapon
    {
        public OneHandSword()
            => _type = eItemType.ONEHANDSWORD;
    }

    public class Shield : Weapon
    {
        public Shield()
            => _type = eItemType.SHIELD;
    }

    public class Dagger : Weapon
    {
        public Dagger()
            => _type = eItemType.DAGGER;
    }

    #endregion

    #region Armour

    public class Helmet : Armour
    {
        public Helmet()
            => _type = eItemType.HELMET;
    }

    public class BodyArmour : Armour
    {
        public BodyArmour()
            => _type = eItemType.BODYARMOUR;
    }

    #endregion
}
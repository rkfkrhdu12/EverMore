using System.Collections.Generic;
using System.Linq;

using GameItem;

public class ItemList
{
    /// <summary>
    /// 아이템 코드 넣으면 아이템 리턴
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

        _codeList[(int)eCodeType.Helmet] = new Dictionary<int, int>();
        _codeList[(int)eCodeType.Bodyarmour] = new Dictionary<int, int>();
        _codeList[(int)eCodeType.Weapon] = new Dictionary<int, int>();

        // CSV Parser
        {
            // 길이가 유동적이고 검색을 하는것이 아니므로 속도가 크게 상관없어 List를 이용
            List<string> itemDatas = CSVParser.Read("ItemList");
            if(null == itemDatas) { return; }

            //itemDatas에서 ','으로 나뉘어진 것을 선택하여 가져온다는 Linq구문이다.
            foreach (var splitDatas in itemDatas.Select(t => t.Split(',')))
            {
                AddList(splitDatas);
            }
        }
    }

    public int GetCodeItemCount(eCodeType codeType) { return _codeList[(int)codeType].Count; }

    #region Variable

    // 아이템은 빨리 자주 찾으며, 키를 통해 찾을것이므로 map(Dictionary)을 이용
    private Dictionary<int, Item> _itemList = new Dictionary<int, Item>();

    // 종류를 번호순으로 정렬하여 저장, ex) _codeList[(int)eCodeType.Weapon][0] == Index와 상관없이 리스트중 첫번째 무기 리턴
    private Dictionary<int, int>[] _codeList = new Dictionary<int, int>[3];

    private ItemAbilityManager _abilMgr = new ItemAbilityManager();

    // Init이 여러번 작동되지 않게끔 막음
    private bool _isInit;

    #endregion

    #region Private Fuction

    private void AddList(IReadOnlyList<string> data)
    {
        if (string.IsNullOrEmpty(data[1])) { return; }

        Item i = null;
        
        switch ((eItemType)int.Parse(data[2]))
        {
            // Weapon
            case eItemType.Sword:                   i = new OneHandSword(); break;
            case eItemType.TwoHandSword:            i = new TwoHandSword(); break;
            case eItemType.Spear:                   i = new Spear();        break;
            case eItemType.Shield:                  i = new Shield();       break;
            case eItemType.Bow:                     i = new Bow();          break;

            // Armour
            case eItemType.Helmet:                  i = new Helmet();       break;
            case eItemType.BodyArmour:              i = new BodyArmour();   break;
        }

        if (i == null)
            return;

        if (i.AniType == eItemType.None)
            return;

        int index = int.Parse(data[1]);

        switch (i.AniType)
        {
            case eItemType.None:        LogMessage.LogError("Item Error" + i.Name); break;
            case eItemType.Helmet:      _codeList[(int)eCodeType.Helmet].Add(_codeList[(int)eCodeType.Helmet].Count, index); break;
            case eItemType.BodyArmour:  _codeList[(int)eCodeType.Bodyarmour].Add(_codeList[(int)eCodeType.Bodyarmour].Count, index); break;
            default:                    _codeList[(int)eCodeType.Weapon].Add(_codeList[(int)eCodeType.Weapon].Count, index); break;
        }

        i.Init(data);

        int abilNum;
        int.TryParse(data[14],out abilNum);

        if (abilNum != 0)
            i.Ability = _abilMgr.GetAbility(abilNum);

        _itemList.Add(index, i);
    }

    #endregion
}

namespace GameItem
{
    public enum eItemType
    {
        None,

        // Armour
        Helmet,
        BodyArmour,

        // Weapon
        Sword = 3,
        NULL,
        Spear,
        Shield,
        Bow,
        TwoHandSword,

        LAST,
    }

    public enum eCodeType
    {
        Helmet,
        Bodyarmour,
        RightWeapon,
        LeftWeapon,
        Weapon = 2,
    }

    public class Item 
    {
        protected string _name;                         public string Name          => _name;
        protected string _itemTypeName;                 public string ItemType      => _itemTypeName;
        protected eItemType _type = eItemType.None;     public eItemType AniType    => _type;

        protected int _cost;                            public int Cost             => _cost;
        protected float _coolTime;                      public float CoolTime       => _coolTime;
        protected int _weight;                          public int Weight           => _weight;
        protected ItemAbility _ability;                 public ItemAbility Ability { get => _ability; set => _ability = value; }

        protected float _range;                         public float AttackRange    => _range;
        protected float _minDamage;                     public float MinDamage      => _minDamage;
        protected float _maxDamage;                     public float MaxDamage      => _maxDamage;
        protected float _speed;                         public float AttackSpeed    => _speed;
        protected float _health;                        public float Health         => _health;
        protected float _defense;                       public float DefensePower   => _defense;

        public virtual void Init(IReadOnlyList<string> datas)
        {
            if (null == datas[1]) { return; }

            _itemTypeName       = datas[3];
            _name               = datas[4];
            int.TryParse(datas[5], out _cost);
            float.TryParse(datas[6], out _coolTime);
            int.TryParse(datas[12], out _weight);

            float.TryParse(datas[7], out _health);
            float.TryParse(datas[8], out _defense);

            float.TryParse(datas[9], out _range);
            float.TryParse(datas[10], out _maxDamage);
            float.TryParse(datas[11], out _minDamage);
            float.TryParse(datas[13], out _speed);
        }

        public virtual void Equip(UnitStatus us)
        {
            us._cost        += _cost;
            us._coolTime    += _coolTime;
            us._weight      += _weight;

            us._maxhealth += _health;
            us._defensivePower += _defense;

            if (_range != -1) us._attackRange = (us._attackRange > _range) ? us._attackRange : _range;

            if (_type <= eItemType.BodyArmour && _type != eItemType.None)
            {
                us._minAttackDamages[0] += _minDamage;
                us._maxAttackDamages[0] += _maxDamage;
            }

            if (_speed != -1) us._attackSpeed = (us._attackSpeed + _speed) / 2;

            us.SetAbility(_ability);
        }

        public virtual void UnEquip(UnitStatus us)
        {
            us._cost        -= _cost;
            us._coolTime    -= _coolTime;
            us._weight      -= _weight;

            // Armour
            us._maxhealth -= _health;
            us._defensivePower -= _defense;

            // Weapon
            if (_range != -1) us._attackRange = (us._attackRange * 2) - _range;
            if (_type <= eItemType.BodyArmour && _type != eItemType.None)
            {
                us._minAttackDamages[2] -= _minDamage;
                us._maxAttackDamages[2] -= _maxDamage;
            }
            if (_speed != -1) us._attackSpeed = (us._attackSpeed * 2) - _speed;

            us.RemoveAbility(_ability);
        }
    }

    public class Weapon : Item
    {

        public override void Init(IReadOnlyList<string> datas)
        {
            base.Init(datas);

        }

        public override void Equip(UnitStatus us)
        {
            base.Equip(us);

            us._minAttackDamage += _minDamage;
            us._maxAttackDamage += _maxDamage;
        }      

        public override void UnEquip(UnitStatus us)
        {
            base.Equip(us);

            us._minAttackDamage -= _minDamage;
            us._maxAttackDamage -= _maxDamage;
        }
    }

    public class Armour : Item
    {

        public override void Init(IReadOnlyList<string> datas)
        {
            base.Init(datas);

        }

        public override void Equip(UnitStatus us)
        {
            base.Equip(us);
        }

        public override void UnEquip(UnitStatus us)
        {
            base.Equip(us);

        }
    }

    #region Weapon

    public class OneHandSword : Weapon
    {
        public OneHandSword()
            => _type = eItemType.Sword;
    }

    public class TwoHandSword : Weapon
    {
        public TwoHandSword()
            => _type = eItemType.TwoHandSword;
    }

    public class Spear : Weapon
    {
        public Spear()
            => _type = eItemType.Spear;
    }

    public class Shield : Weapon
    {
        public Shield()
            => _type = eItemType.Shield;
    }

    public class Bow : Weapon
    {
        public Bow()
            => _type = eItemType.Bow;
    }

    #endregion

    #region Armour

    public class Helmet : Armour
    {
        public Helmet()
            => _type = eItemType.Helmet;
    }

    public class BodyArmour : Armour
    {
        public BodyArmour()
            => _type = eItemType.BodyArmour;
    }

    #endregion


}
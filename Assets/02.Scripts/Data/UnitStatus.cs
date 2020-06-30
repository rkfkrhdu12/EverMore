using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;
using System.Runtime.Remoting.Messaging;



public struct UnitStatus
{
    public eTeam _team;

    public float _curhealth;
    public float _maxhealth;
    public float _defensivePower;

    public float[] _maxAttackDamages;    public float AttackDamage { get { return LeftAttackDamage + RightAttackDamage; } }
    public float[] _minAttackDamages;    public float LeftAttackDamage      { get { return Random.Range(_minAttackDamages[0], _maxAttackDamages[0]) + ((_minAttackDamages[1] == 0 && _maxAttackDamages[1] == 0) ? _minAttackDamages[2] : _minAttackDamages[2] / 2.0f); } }
                                         public float RightAttackDamage     { get { return Random.Range(_minAttackDamages[1], _maxAttackDamages[1]) + ((_minAttackDamages[0] == 0 && _maxAttackDamages[0] == 0) ? _minAttackDamages[2] : _minAttackDamages[2] / 2.0f); } }

    #region Set AttackDamage

    private int _maxDamageIndex;
    private int _minDamageIndex;

    public float _minAttackDamage
    {
        get
        {
            if (_minAttackDamages.Length - 1 > _minDamageIndex)
            {
                return _minAttackDamages[_minDamageIndex];
            }

            return 0.0f;
        }

        set
        {
            if (_minAttackDamages == null) { return; }

            if (_minAttackDamages.Length > _minDamageIndex)
            {
                _minAttackDamages[_minDamageIndex++] = value;
            }
        }
    }
    public float _maxAttackDamage
    {
        get
        {
            if (_minAttackDamages.Length - 1 > _maxDamageIndex)
            {
                return _minAttackDamages[_maxDamageIndex];
            }

            return 0.0f;
        }

        set
        {
            if (_maxAttackDamages == null) { return; }

            if (_maxAttackDamages.Length > _maxDamageIndex)
            {
                _maxAttackDamages[_maxDamageIndex++] = value;
            }
        }
    }
    #endregion

    public ItemAbility[] _abilities;     public ItemAbility[] Abilities { get { return _abilities; }  }
        
    #region Set Ability

    private int _abilIndex;
    public void SetAbility(ItemAbility abil)
    {
        if (_abilities == null) { return; }
        if (_abilities.Length <= _abilIndex) { return; }

        _abilities[_abilIndex++] = abil;
    }

    public void RemoveAbility(ItemAbility abil)
    {
        if (_abilities == null) { return; }
        for (int i = 0; i < 4; ++i)
        {
            if (_abilities[i] == null) { continue; }

            if( _abilities[i].Code == abil.Code) { _abilities[i] = null; }
        }
    }
    #endregion

    public float _attackSpeed;      public float AttackSpeed => _attackSpeed;
    public float _attackRange;      

    public float _moveSpeed;
    public int _cost;
    public float _coolTime;
    public int _weight;

    public int[] _equipedItems;

    public void UpdateItems()
    {
        {
            _maxhealth = 0f;
            _defensivePower = 0f;
            _moveSpeed = 3.5f;
            _cost = 0;
            _coolTime = 0f;
            _weight = 0;
            _minDamageIndex = 0;
            _maxDamageIndex = 0;
            _maxAttackDamages = new float[3];
            _minAttackDamages = new float[3];
            _abilIndex = 0;
            _abilities = new ItemAbility[4];
            _attackRange = 0f;
            _attackSpeed = 0f;
        }

        ItemList itemList = Manager.Get<GameManager>().itemList;

        for (int i = 0; i < _equipedItems.Length; ++i)
        {
            if(_equipedItems[i] == 0) { if (i == 2 || i == 3) { ++_maxDamageIndex; ++_minDamageIndex; } continue; }

            itemList.ItemSearch(_equipedItems[i]).Equip(ref this);
        }

        _curhealth = _maxhealth;

        if (_attackRange == 0) { _attackRange = 1f; }
        if (_attackSpeed == 0) { _attackSpeed = 1f; }

    }

    public void Init(eTeam team = eTeam.PLAYER)
    {
        _team = team;
        _maxhealth = 0f;
        _defensivePower = 0f;
        _moveSpeed = 3.5f;
        _cost = 0;
        _coolTime = 0f;
        _weight = 0;
        _maxAttackDamages = new float[2];
        _minAttackDamages = new float[2];
        _attackRange = 1f;
        _attackSpeed = 1f;

        _equipedItems = new int[4];

        ItemList itemList = Manager.Get<GameManager>().itemList;

        _equipedItems[0] = itemList.CodeSearch(GameItem.eCodeType.Helmet,0);
        _equipedItems[1] = itemList.CodeSearch(GameItem.eCodeType.Bodyarmour, 0);
    }
}

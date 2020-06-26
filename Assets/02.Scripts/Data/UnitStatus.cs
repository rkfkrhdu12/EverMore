using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;
using System.Runtime.Remoting.Messaging;



public struct UnitStatus
{
    public eTeam _team;

    public float _maxhealth;
    public float _defensivePower;

    #region Set AttackDamage

    private int _maxDamageIndex;
    public float[] _maxAttackDamages;

    private int _minDamageIndex;
    public float[] _minAttackDamages;

    public float _minAttackDamage
    {
        get
        {
            if (_minAttackDamages == null) { return 0; }

            float returnVal = _minAttackDamages.Length <= _minDamageIndex ?
                _minAttackDamages[0] + _minAttackDamages[1] :
                _minAttackDamages[_minDamageIndex];

            return returnVal;
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
            if (_maxAttackDamages == null) { return 0; }

            float returnVal = _maxAttackDamages.Length <= _maxDamageIndex ?
                _maxAttackDamages[0] + _maxAttackDamages[1] :
                _maxAttackDamages[_maxDamageIndex];

            return returnVal;
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

    public float _attackDamage          { get { return _leftAttackDamage + _rightAttackDamage; } }
    public float _leftAttackDamage      { get { return Random.Range(_minAttackDamages[0], _maxAttackDamages[0]); } }
    public float _rightAttackDamage     { get { return Random.Range(_minAttackDamages[1], _maxAttackDamages[1]); } }

    public float _attackSpeed;
    public float _attackRange;

    public float _moveSpeed;
    public int _cost;
    public float _coolTime;
    public int _weight;

    public int[] _equipedItems;

    public void ChangeItem(int curCode, int changeCode)
    {
        ItemList itemList = Manager.Get<GameManager>().itemList;

        _minDamageIndex = 0;
        _maxDamageIndex = 0;
        for (int i = 0; i < 4; ++i)
        {
            if (curCode == _equipedItems[i])
            {
                itemList.ItemSearch(_equipedItems[i]).UnEquip(ref this);

                _equipedItems[i] = changeCode;

                itemList.ItemSearch(_equipedItems[i]).Equip(ref this);
                return;
            }
        }
    }

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
            _maxAttackDamages = new float[2];
            _minAttackDamages = new float[2];
            _attackRange = 0f;
            _attackSpeed = 0f;
        }

        ItemList itemList = Manager.Get<GameManager>().itemList;

        for (int i = 0; i < _equipedItems.Length; ++i)
        {
            if(_equipedItems[i] == 0) { if (i == 2 || i == 3) { ++_maxDamageIndex; ++_minDamageIndex; } continue; }

            itemList.ItemSearch(_equipedItems[i]).Equip(ref this);
        }

        if(_attackRange == 0) { _attackRange = 1f; }
        if(_attackSpeed == 0) { _attackSpeed = 1f; }
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

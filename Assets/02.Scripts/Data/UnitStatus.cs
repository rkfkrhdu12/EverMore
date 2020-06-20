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

    private int _damageIndex;
    public float[] _attackDamages;
    public float _attackDamage 
    {
        get 
        {
            if(_attackDamages == null) { return 0; }

            float returnVal = _attackDamages.Length <= _damageIndex ? 
                _attackDamages[0] + _attackDamages[1] : 
                _attackDamages[_damageIndex];

            return returnVal;
        }

        set
        {
            if (_attackDamages == null) { return; }

            if (_attackDamages.Length > _damageIndex)
            {
                _attackDamages[_damageIndex++] = value;
            }
        }
    }
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
            _attackDamages = new float[2];
            _attackRange = 2f;
            _attackSpeed = 1f;
        }

        ItemList itemList = Manager.Get<GameManager>().itemList;

        for (int i = 0; i < _equipedItems.Length; ++i)
        {
            if(_equipedItems[i] == 0) { if (i == 2 || i == 3) { ++_damageIndex; } continue; }

            itemList.ItemSearch(_equipedItems[i]).Equip(ref this);

        }
    }

    public void Init(eTeam team = eTeam.PLAYER)
    {
        _team = team;
        _maxhealth = 0f;
        _defensivePower = 0f;
        _moveSpeed = 0f;
        _cost = 0;
        _coolTime = 0f;
        _weight = 0;
        _attackDamage = 0f;
        _attackRange = 1f;
        _attackSpeed = 1f;

        _equipedItems = new int[4];
        _equipedItems[0] = 1;
        _equipedItems[1] = 2;
    }
}

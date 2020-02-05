using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // H

    #region Variable

    public int _curhealth;
    public int _maxhealth;

    // 방어력
    public float _defensivePower;

    public (float _damage, float _speed, float _range) _attack;
    //public int _attackDamage;
    //public float _attackSpeed;
    //public float _attackRange;

    public float _moveSpeed;

    public eTeam _team;
    public enum eTeam
    {
        PLAYER,
        ENEMY,
    }

    // 유닛 코스트
    public int _cost;
    // second
    public float _coolTime;

    // 파일 파싱으로 아이디 가져온 후 적용시킬 예정
    // 특성 번호
    public int _abilityNum;

    // 아이템 번호
    public int[] _itemsNum = new int[4];
    public enum eEquipItem
    {
        HELMET,
        ARMOUR,
        WEAPON,
        SHIELD
    }

    private bool _isdead = true;

    #endregion

    #region Private Fuction

    private void FixedUpdate()
    {
        if (!_isdead) return;

        transform.Translate(0, 0, _moveSpeed * Time.deltaTime);
    }

    #endregion

    public void Spawn()
    {
        _isdead = false;
    }
    public bool GetIsDead() 
    // 윤용우 생성
    {
        return _isdead;
    }
    public void SetIsDead(bool dead)
    // 윤용우 생성
    {
        _isdead = dead;
    }
    public void Init(int curH = 100, int maxH = 100, int speed = 3, eTeam team = eTeam.PLAYER) 
    // 윤용우 생성
    {
        _curhealth = curH;
        _maxhealth = maxH;
        _abilityNum = 1;
        _attack = (10, 2, 3);
        _coolTime = 1;
        _cost = 10;
        _defensivePower = 10;
        _moveSpeed = speed;
        _team = team;
    }
    // 추후 삭제예정
    static Unit TestInit()
    {
        Unit retval = new Unit();
        retval._curhealth = 100;
        retval._maxhealth = 100;
        retval._abilityNum = 1;
        retval._attack = (10, 2, 3);
        retval._coolTime = 1;
        retval._cost = 10;
        retval._defensivePower = 10;
        retval._itemsNum[(int)eEquipItem.HELMET] = 1;
        retval._itemsNum[(int)eEquipItem.ARMOUR] = 2;
        retval._itemsNum[(int)eEquipItem.SHIELD] = 0;
        retval._itemsNum[(int)eEquipItem.WEAPON] = 1;
        retval._moveSpeed = 3;
        retval._team = eTeam.PLAYER;

        return retval;
    }
}

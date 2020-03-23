using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AnimatorPro;

public enum eTeam
{
    PLAYER,
    ENEMY,
}

public struct UnitStatus
{
    public float    maxhealth;
    public float    defensivePower;

    public float    moveSpeed;
    public int      cost;
    public float    coolTime;
    public int      weight;
}

public class Unit : FieldObject
{
    #region Variable

    #region Status
        // 추후 모두 private

    //public float _curhealth;
    //public float _maxhealth;

    // 방어력
    public float _defensivePower;
    public float _attackDamage;
    public float _attackSpeed;
    public float _attackRange;

    public float _moveSpeed;
    // 유닛 코스트
    public int _cost;
    // second
    public float _coolTime;
    #endregion

    public int _abilityNum;

    //public eTeam _team;

    // 파일 파싱으로 아이디 가져온 후 적용시킬 예정
    // 특성 번호

    // 아이템 번호
    public int[] _itemsNum = new int[4];
    public enum eEquipItem
    {
        HELMET,
        ARMOUR,
        WEAPON,
        SUBWEAPON,
    }

    eState _curState = eState.MOVE;
    private enum eState
    {
        IDLE,
        MOVE,
        ATTACK,
    }

    // public으로 해놔야함 Inspector를 통해 값을 대입
    public SphereCollider _collider;

    public Queue<FieldObject> _attackTargets = new Queue<FieldObject>();
    FieldObject _curTarget = null;
    public float _attackTime = 0.0f;

    //public bool _isdead = true;

    Animator _animator;
    AnimatorPro _aniPro;

    Rigidbody _rigid;

    // Test
    private static readonly int _idAttack   = Animator.StringToHash("Attack");
    private static readonly int _idMove     = Animator.StringToHash("Move");

    #endregion

    #region Private Fuction

    private void FixedUpdate()
    {
        if (_isdead) return;

        UpdateState();

        UpdateMonobehaviour();
    }

    private void UpdateState()
    {
        // 설정된 타겟도 설정되어야할 타겟도 없으면 eState.Move
        if (null == _curTarget && 0 == _attackTargets.Count)
        {
            _curState = eState.MOVE;
        }
        else
        { // 설정된 타겟 혹은 설정되어야할 타겟이 잇으므로 eState.Attack
            _curState = eState.ATTACK;
        }
    }

    private void UpdateMonobehaviour()
    {
        switch(_curState)
        {
            case eState.MOVE:   UpdateMove();   break;
            case eState.ATTACK: UpdateAttack(); break;
        }
    }

    private void UpdateMove()
    {
        transform.Translate(0, 0, _moveSpeed * Time.deltaTime);

        // Test
        _aniPro.SetParam(_idMove, _rigid.velocity.z);
    }

    private void UpdateAttack()
    {
        if (_attackTime <= _attackSpeed)
        {
            _attackTime += Time.deltaTime;
        }
        else
        {
            // 타겟이 없엇거나 체력이 0 이하로 내려가면 새로운 타겟을 정한다.
            if (null == _curTarget || 0 >= _curTarget._curhealth)
            {
                if(0 == _attackTargets.Count) { _curTarget = null; return; }

                _curTarget = _attackTargets.Dequeue();
            }

            _attackTime = 0.0f;

            _curTarget.DamageReceive(_attackDamage);
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) { return; }
        if(!other.CompareTag("Unit")) { return; }

        FieldObject target = other.GetComponent<FieldObject>();
        if (_attackTargets.Contains(target) || _team == target._team) { return; }

        // Test
        _aniPro.SetParam(_idAttack, true);

        _attackTargets.Enqueue(target);
    }

    public void Spawn()
    {
        _isdead = false;

        if(null == _collider) { _collider = GetComponent<SphereCollider>(); }
        _collider.radius *= _attackRange;

        #region Item
        GameObject _helmetObj = GameSystem.Instance.itemList.ItemSearch(_itemsNum[0]).Object;
        Instantiate(_helmetObj, gameObject.transform);

        GameObject _armourObj = GameSystem.Instance.itemList.ItemSearch(_itemsNum[1]).Object;
        Instantiate(_armourObj, gameObject.transform);

        if (0 != _itemsNum[2])
        {
            GameObject _weaponObj = GameSystem.Instance.itemList.ItemSearch(_itemsNum[2]).Object;
            Instantiate(_weaponObj, gameObject.transform);
        }

        if (0 != _itemsNum[3])
        {
            GameObject _subweaponObj = GameSystem.Instance.itemList.ItemSearch(_itemsNum[3]).Object;
            Instantiate(_subweaponObj, gameObject.transform);
        }
        #endregion

        _animator = GetComponent<Animator>();
        if (null == _animator) { }

        _aniPro = GetComponent<AnimatorPro>();
        if (null == _aniPro) { }

        _aniPro?.Init(_animator);

        _rigid = GetComponent<Rigidbody>();
    }

    public override  void DamageReceive(float damage)
    {
        _curhealth -= damage;

        if(_curhealth <= 0)
        {
            _isdead = true;

            DeleteObjectManager.AddDeleteObject(gameObject);
        }
    }

    public void Equip(int code, eEquipItem weapon = eEquipItem.WEAPON)
    {
        Item i = GameSystem.Instance.itemList.ItemSearch(code);

        if (null == i) { return; }

        UnitStatus us = new UnitStatus();

        i.Equip(ref us);

        _maxhealth          += us.maxhealth;
        _curhealth          = _maxhealth;
        _defensivePower     += us.defensivePower;
        _moveSpeed          += us.moveSpeed;
        _cost               += us.cost;
        _coolTime           += us.coolTime;

        switch (i.Type)
        {
            case eItemType.NONE:            { break; }
            case eItemType.HELMET:          { _itemsNum[(int)eEquipItem.HELMET] = code; break; }
            case eItemType.BODYARMOUR:      { _itemsNum[(int)eEquipItem.ARMOUR] = code; break; }
            default:                        { _itemsNum[(int)weapon] = code; break; }
        }
    }

    public void Init(Unit unit)
    {
        Init();

        Equip(unit._itemsNum[0]);                        // = unit._itemsNum[0];
        Equip(unit._itemsNum[1]);                        // = unit._itemsNum[1];
        Equip(unit._itemsNum[2]);                        // = unit._itemsNum[2];
        Equip(unit._itemsNum[3],eEquipItem.SUBWEAPON);   // = unit._itemsNum[3];
    }

    public void Init(int curH = 100, int maxH = 100, int speed = 3, eTeam team = eTeam.PLAYER)
    // 윤용우 생성
    {
        _curhealth = curH;
        _maxhealth = maxH;
        _abilityNum = 1;
        _attackDamage = 10;
        _attackSpeed = 2;
        _attackRange = 3;
        _coolTime = 0;
        _cost = 0;
        _defensivePower = 0;
        _moveSpeed = speed;
        _team = team;
    }

}

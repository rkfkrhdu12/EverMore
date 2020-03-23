using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTeam
{
    PLAYER,
    ENEMY,
}

public class Unit : MonoBehaviour
{
    // H

    #region Variable

    #region Status
        // 추후 모두 private

    public float _curhealth;
    public float _maxhealth;

    // 방어력
    public float _defensivePower;
    public float _attackDamage;
    public float _attackInterval;
    public float _attackRange;

    public float _moveSpeed;
    // 유닛 코스트
    public int _cost;
    // second
    public float _coolTime;
    #endregion

    public eTeam _team;

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
        WEAPON2,
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

    public Queue<Unit> _attackTargets = new Queue<Unit>();
    Unit _curTarget = null;
    public float _attackTime = 0.0f;

    public bool _isdead = true;

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
    }

    private void UpdateAttack()
    {
        if (_attackTime <= _attackInterval)
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

        Unit target = other.GetComponent<Unit>();
        if (_attackTargets.Contains(target) || _team == target._team) { return; }

        _attackTargets.Enqueue(target);
    }

    public void Spawn()
    {
        _isdead = false;

        if(null == _collider) { _collider = GetComponent<SphereCollider>(); }
        _collider.radius *= _attackRange;
    }

    public void DamageReceive(float damage)
    {
        //
        _curhealth -= damage;
    }

    public bool IsDead { get { return _isdead; } set { } }

    public void Init(int curH = 100, int maxH = 100, int speed = 3, eTeam team = eTeam.PLAYER) 
    // 윤용우 생성
    {
        _curhealth = curH;
        _maxhealth = maxH;
        _abilityNum = 1;
        _attackDamage = 10;
        _attackInterval = 2;
        _attackRange = 3;
        _coolTime = 1;
        _cost = 10;
        _defensivePower = 10;
        _moveSpeed = speed;
        _team = team;
    }
}

using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.AnimatorPro;

using GameplayIngredients;

public enum eTeam
{
    PLAYER,
    ENEMY
}

public class UnitModelManager
{
    public void UpdateModel(GameObject unit, in int[] equipedItem)
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        for (int i = 0; i < 2; ++i)
        {
            string itemName = Manager.Get<GameManager>().itemList.ItemSearch(equipedItem[i]).Name;

            int[] index = _modelItemPoint?[itemName];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(true);
        }
    }

    #region Private Variable

    // 아이템 이름 > 오브젝트에서의 아이템 위치
    private Dictionary<string, int[]> _modelItemPoint = new Dictionary<string, int[]>();

    private const int _completeModelCount = 4;

    #endregion

    #region Private Function

    private void Init()
    {
        string[] itemNameList = new string[_completeModelCount * 2];
        int itemIndex = 0;
        itemNameList[itemIndex++] = "일반 머리";
        itemNameList[itemIndex++] = "일반 옷";
        itemNameList[itemIndex++] = "견습 기사의 투구";
        itemNameList[itemIndex++] = "견습 기사의 갑옷";
        itemNameList[itemIndex++] = "셔우드 숲의 모자";
        itemNameList[itemIndex++] = "셔우드 숲의 코트";
        itemNameList[itemIndex++] = "하얀 눈의 모자";
        itemNameList[itemIndex++] = "하얀 눈의 옷";

        int equipmentCount = 2;

        int[] modelNumList = new int[_completeModelCount * 2];
        for (int i = 1; i < _completeModelCount * 2 + 1; ++i)
        {
            modelNumList[i - 1] = (1 == i % 2 ? i / 2 : i / 2 - 1);
        }

        int[] v = new int[equipmentCount];
        for (int i = 0; i < itemIndex; ++i)
        {
            v = new int[equipmentCount];
            v[0] = modelNumList[i];
            v[1] = i % equipmentCount;

            _modelItemPoint.Add(itemNameList[i], v);
        }
    }

    #endregion
}

public struct UnitStatus
{
    public float _maxhealth;
    public float _curhealth;
    public float _defensivePower;

    public float _attackDamage;
    public float _attackSpeed;
    public float _attackRange;

    public float _moveSpeed;
    public int _cost;
    public float _coolTime;
    public int _weight;

    public int[] _equiedItems;

    public void Reset()
    {
        _maxhealth = 0f;
        _defensivePower = 0f;
        _moveSpeed = 0f;
        _cost = 0;
        _coolTime = 0f;
        _weight = 0;
        _equiedItems = new int[4];
    }
}

public class UnitController : FieldObject
{
    public void Spawn()
    {
        //콜라이더 비가 비어있다면, 가져온다.
        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        //공격 영역 사이즈를 공격 범위에 영향을 준다. 
        _collider.size *= _status._attackRange;

        #region Item
        ////아이템 번호가 0이 아니라면, 헬멧을 검색하여 생성합니다.
        //if (_itemsNum[0] != 0)
        //{
        //    var _helmetObj = Manager.Get<GameManager>().itemList.ItemSearch(_itemsNum[0]).Object;
        //    Instantiate(_helmetObj, gameObject.transform);
        //}
        //
        ////아이템 번호가 0이 아니라면, 아머를 검색하여 생성합니다.
        //if (_itemsNum[1] != 0)
        //{
        //    var _armourObj = Manager.Get<GameManager>().itemList.ItemSearch(_itemsNum[1]).Object;
        //    Instantiate(_armourObj, gameObject.transform);
        //}
        //
        ////아이템 번호가 0이 아니라면, 무기를 검색하여 생성합니다.
        //if (_itemsNum[2] != 0)
        //{
        //    // var hands = transform.GetChild(0).GetComponent<UnitWeaponHand>();
        //    //
        //    // if (hands != null)
        //    // {
        //    //     var _weaponObj = Manager.Get<GameManager>().itemList.ItemSearch(_itemsNum[2]).Object;
        //    //     Instantiate(_weaponObj, hands._RightHand.transform);
        //    // }
        //}
        //
        ////아이템 번호가 0이 아니라면, 서브 무기를 검색하여 생성합니다.
        //if (_itemsNum[3] != 0)
        //{
        //    var _subweaponObj = Manager.Get<GameManager>().itemList.ItemSearch(_itemsNum[3]).Object;
        //    Instantiate(_subweaponObj, gameObject.transform);
        //}
        //
        #endregion

        _animator = transform.GetChild(0).GetComponent<Animator>();

        _aniPro = transform.GetChild(0).GetComponent<AnimatorPro>();

        _aniPro.Init(_animator);

        _aniPro.SetParam(_idAttackSpd, 1 / _attackSpeed);
    }

    public override void DamageReceive(float damage)
    {
        //데미지를 받습니다.
        _curHp -= damage;

        //체력이 0 초과라면 : 아래 코드 구문 실행 x
        if (_curHp > 0) return;

        //사망 처리 한다.
        _isDead = true;

        //해당 유닛을 삭제 목록에 올립니다.
        DeleteObjectSystem.AddDeleteObject(gameObject);
    }

    // //창작 함수
    //public void Equip(int code, eEquipItem weapon = eEquipItem.WEAPON)
    //{
    // //코드를 기반하여 아이템을 검색하여 할당 받습니다.
    // var item = Manager.Get<GameManager>().itemList.ItemSearch(code);
    //
    // //받아와지지 않았다면 : return
    // if (item == null)
    //     return;
    //
    // //유닛 상태 객체 생성
    // var unitStatus = new UnitStatus();
    //
    // //아이템을 장착합니다.
    // item.Equip(ref unitStatus);
    //
    // //능력 처리를 함.
    // _maxHp += unitStatus.maxhealth;
    // _curHp = _maxHp;
    // _defensivePower += unitStatus.defensivePower;
    // _moveSpeed += unitStatus.moveSpeed;
    // _cost += unitStatus.cost;
    // _coolTime += unitStatus.coolTime;
    //
    // //헬멧과 아머, 무기에 코드를 대입
    // switch (item.Type)
    // {
    //     case eItemType.HELMET:
    //         _itemsNum[(int) eEquipItem.HELMET] = code;
    //         break;
    //     case eItemType.BODYARMOUR:
    //         _itemsNum[(int) eEquipItem.ARMOUR] = code;
    //         break;
    //     default:
    //         _itemsNum[(int) weapon] = code;
    //         break;
    // }
    //}

    ////유닛 데이터를 가지고 초기화를 합니다.
    //public void Init(UnitController unit)
    //{
    //    //기본 초기화
    //    Init();

    //    //장착한 아이템을 기반으로 초기화
    //    Equip(unit._itemsNum[0]);
    //    Equip(unit._itemsNum[1]);
    //    Equip(unit._itemsNum[2]);
    //    Equip(unit._itemsNum[3], eEquipItem.SUBWEAPON);

    //    //팀 처리
    //    eteam = unit.eteam;
    //}

    ////기본 초기화를 합니다.
    //public void Init(int curH = 100, int maxH = 100, int speed = 3)
    //{
    //    _curHp = 0;
    //    _maxHp = 0;
    //    _abilityNum = 0;
    //    _attackDamage = 5f;
    //    _attackSpeed = 1f;
    //    _attackRange = 1f;
    //    _coolTime = 0;
    //    _cost = 0;
    //    _defensivePower = 0;
    //    _moveSpeed = speed;
    //    //_3DModelToTextureName = "Naked-head,Naked-body";

    //    _itemsNum[0] = 1;
    //    _itemsNum[1] = 2;
    //    for (int i = 2; i < _itemsNum.Length; ++i)
    //        _itemsNum[i] = 0;
    //}

    #region Enum

    public enum eEquipItem
    {
        HELMET,
        ARMOUR,
        WEAPON,
        SUBWEAPON,
    }

    private enum eState
    {
        NONE,
        IDLE,
        MOVE,
        ATTACK,
    }

    #endregion

    #region Macro

    private static readonly int _idAttack = Animator.StringToHash("Attack");
    private static readonly int _idAttackSpd = Animator.StringToHash("AttackSpeed");
    private static readonly int _idMove = Animator.StringToHash("Move");

    #endregion

    #region Show Inspector

    //아군인지, 적인지에 대한 변수
    public eTeam eteam;

    //능력 번후
    public int _abilityNum;

    // 공격 범위일듯.
    public BoxCollider _collider;

    //공격 타겟에대한 큐
    public Queue<FieldObject> _attackTargets = new Queue<FieldObject>();

    //공격 시간에 대한 변수
    public float _attackTime;

    //죽었는지 살았는지에 대한 변수
    public bool _isDead;

    //[Header("파일 파싱으로 아이디 가져온 후 적용시킬 예정")]
    // 아이템 번호
    public int[] _itemsNum => _status._equiedItems;

    // public string _3DModelToTextureName = "Naked-head,Naked-body";
    #endregion

    #region Hide Inspector

    #region 유닛 상태

    public UnitStatus _status;

    //방어력
    public float _defensivePower { get { return _status._defensivePower; } }

    //공격 데미지
    public float _attackDamage { get { return _status._attackDamage; } }

    //공격 속도
    public float _attackSpeed { get { return _status._attackSpeed; } }

    //공격 범위
    public float _attackRange { get { return _status._attackRange; } }

    //이동 속도
    public float _moveSpeed { get { return _status._moveSpeed; } }

    // 유닛 코스트
    public int _cost { get { return _status._cost; } }

    // second
    public float _coolTime { get { return _status._coolTime; } }

    #endregion

    #region Other

    //애니메이터 관련 변수
    private Animator _animator;
    private AnimatorPro _aniPro;

    //리지드 바디 변수
    private Rigidbody _rig;

    //유닛 상태에 대한 변수
    private eState _curState = eState.NONE;

    //필드 관점에서의 해당 유닛의 상태
    private FieldObject _curTarget;

    //이동에 대한 파라미터
    private float moveParameter;

    #endregion

    #endregion

    #region Monobehaviour Function
    private void FixedUpdate()
    {
        //현재 상태가 비어 있거나, 죽었다면 : return
        if (_curState == eState.NONE || _isDead) return;

        //이동 <--> 공격, 상태 변환
        UpdateState();

        //상태 변수를 통한, 유닛 업데이트
        UpdateUnit();
    }

    private void OnTriggerEnter(Collider other)
    {
        //닿은 생대가 유닛이 아니라면 : 아래 코드 구문 실행 X
        if (!other.CompareTag("Unit"))
            return;

        //other의 필드 관점의 데이터를 가져옴.
        var target = other.GetComponent<FieldObject>();

        //공격할 타겟 큐에 해당 타겟이 있거나, 타겟의 팀이 우리 팀이라면,
        if (_attackTargets.Contains(target) || target._team == eteam)
            return;

        //공격 타겟에 해당 타겟을 넣어줍니다.
        _attackTargets.Enqueue(target);

        _aniPro?.SetParam(_idAttack, true);
    }

    #endregion

    #region Private Function

    private void UpdateState()
    {
        // 설정된 타겟이 없거나, 설정되어야할 타겟도 없으면 : eState.Move
        if (_curTarget == null && _attackTargets.Count == 0)
            _curState = eState.MOVE;
        else
            // 설정된 타겟 혹은 설정되어야할 타겟이 있으므로 : eState.Attack
            _curState = eState.ATTACK;
    }

    private void UpdateUnit()
    {
        moveParameter = 0;

        switch (_curState)
        {
            case eState.MOVE:
            UpdateMove();
            break;
            case eState.ATTACK:
            UpdateAttack();
            break;
        }

        UpdateAnimation();
    }

    private void UpdateMove()
    {
        moveParameter = 1;

        transform.Translate(0, 0, _moveSpeed * Time.deltaTime);
    }

    private void UpdateAttack()
    {
        //공격 시간이 공격 속도보다 같거나 낮다면 : 공격 시간을 높혀준다.
        if (_attackTime <= _attackSpeed)
            _attackTime += Time.deltaTime;
        else
        {
            // 타겟이 없엇거나, 체력이 0 이하로 내려가면 : 새로운 타겟을 정한다.
            if (_curTarget == null || _curTarget._curHp <= 0)
            {
                //공격할 타겟이 0명이라면 : return
                if (_attackTargets.Count == 0)
                {
                    _curTarget = null;
                    return;
                }

                //현재 타겟을 Dequeue한다.
                _curTarget = _attackTargets.Dequeue();
            }

            //공격 시간 초기화
            _attackTime = 0f;

            //데미지 리시브
            _curTarget.DamageReceive(_attackDamage);
        }
    }

    private void UpdateAnimation()
    {
        if (_aniPro == null) return;

        _aniPro.SetParam(_idMove, moveParameter);

        //공격 중이 아니라면 : return
        if (!_aniPro.GetParam<bool>(_idAttack)) return;

        //타겟이 있거나, 타겟의 체력이 0 초과라면 : return
        if (_curTarget != null && _curTarget._curHp > 0) return;

        //공격할 타겟이 0명이라면 : 공격을 중지함.
        if (_attackTargets.Count == 0) _aniPro.SetParam(_idAttack, false);
    }

    #endregion
}

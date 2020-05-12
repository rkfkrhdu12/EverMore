using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.AnimatorPro;
using UnityEngine.AI;

public enum eTeam
{
    PLAYER,
    ENEMY
}

public class UnitController : FieldObject
{
    public void Spawn()
    {
        //콜라이더 비가 비어있다면, 가져온다.
        if (_attackAreaCollider == null)
            _attackAreaCollider = GetComponent<BoxCollider>();

        _status.UpdateItems();

        //공격 영역 사이즈를 공격 범위에 영향을 준다. 
        _attackAreaCollider.size *= _status._attackRange;

        _animator = transform.GetChild(0).GetComponent<Animator>();

        _aniPro = transform.GetChild(0).GetComponent<AnimatorPro>();

        _aniPro.Init(_animator);

        _aniPro.SetParam(_idAttackSpd, 1 / _attackSpeed);

        _navMeshAgent.updateRotation = false;

        _curState = eState.IDLE;
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

    #region Enum

    public enum eEquipItem
    {
        HELMET,
        ARMOUR,
        WEAPON,
        SUBWEAPON,
    }

    public enum eState
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
    //능력 번후
    [HideInInspector]
    public int _abilityNum;

    // 공격 범위일듯.
    public BoxCollider _attackAreaCollider;

    //공격 타겟에대한 큐
    public Queue<FieldObject> _attackTargets = new Queue<FieldObject>();

    //공격 시간에 대한 변수
    private float _attackTime;

    //[Header("파일 파싱으로 아이디 가져온 후 적용시킬 예정")]
    // 아이템 번호
    public int[] _itemsNum => _status._equipedItems;

    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    // 상대방 진영의 성
    public Vector3 _enemyCastlePosition;

    public Transform _enemyCastleTrs;

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
    public eState _curState = eState.NONE;

    //필드 관점에서의 해당 유닛의 상태
    public FieldObject _curTarget = null;

    //이동에 대한 파라미터
    private float moveParameter;

    #endregion

    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        Spawn();
    }

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
        if (_attackTargets.Contains(target) || target._team == _team)
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
        _navMeshAgent.SetDestination(
            //_curTarget == null ?
            _enemyCastleTrs.position);// :
            //_curTarget.transform.position);

        //moveParameter = 1;

        //transform.Translate(0, 0, _moveSpeed * Time.deltaTime);
    }

    private void UpdateAttack()
    {
        // 타겟이 없엇거나, 체력이 0 이하로 내려가면 : 새로운 타겟을 정한다.
        if (_curTarget == null || _curTarget._curHp <= 0)
        {
            //공격할 타겟이 0명이라면 : null(상대 성이 타겟) 아니면 그 대상에게 전진
            if (_attackTargets.Count == 0) { _curTarget = null; }
            //현재 타겟을 Dequeue한다.
            else { _curTarget = _attackTargets.Dequeue(); }
        }

        ////공격 시간이 공격 속도보다 같거나 낮다면 : 공격 시간을 높혀준다.
        //if (_attackTime <= _attackSpeed)
        //    _attackTime += Time.deltaTime;
        //else
        //{
        //    // 타겟이 없엇거나, 체력이 0 이하로 내려가면 : 새로운 타겟을 정한다.
        //    if (_curTarget == null || _curTarget._curHp <= 0)
        //    {
        //        //공격할 타겟이 0명이라면 : return
        //        if (_attackTargets.Count == 0)
        //        {
        //            _curTarget = _lastTarget;
        //            return;
        //        }

        //        //현재 타겟을 Dequeue한다.
        //        _curTarget = _attackTargets.Dequeue();
        //    }

        //    //공격 시간 초기화
        //    _attackTime = 0f;

        //    //데미지 리시브
        //    _curTarget.DamageReceive(_attackDamage);
        //}
    }

    private void UpdateAnimation()
    {
        if (_aniPro == null) return;

        _aniPro.SetParam(_idMove, 1.0f);

        if (_navMeshAgent.velocity.sqrMagnitude >= .1f * .1f && _navMeshAgent.remainingDistance <= .1f)
        {
            _aniPro.SetParam(_idMove, 0);
        }
        else if (_navMeshAgent.desiredVelocity.sqrMagnitude >= .1f* .1f)
        {
            Vector3 direction = _navMeshAgent.desiredVelocity;

            Quaternion targetAngle = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  targetAngle,
                                                  Time.deltaTime * 8.0f);

        }

        ////공격 중이 아니라면 : return
        //if (!_aniPro.GetParam<bool>(_idAttack)) return;

        ////타겟이 있거나, 타겟의 체력이 0 초과라면 : return
        //if (_curTarget != null && _curTarget._curHp > 0) return;

        ////공격할 타겟이 0명이라면 : 공격을 중지함.
        //if (_attackTargets.Count == 0) _aniPro.SetParam(_idAttack, false);
    }

    #endregion
}

public class UnitModelManager
{
    public static void ResetModel(GameObject unit, in int[] equipedItems)
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        for (int i = 0; i < 2; ++i)
        {
            if(0 == equipedItems[i]) { continue; }
            string itemName = _itemList.ItemSearch(equipedItems[i]).Name;

            int[] index = _modelItemPoint?[itemName];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(false);
        }
    }

    public static void UpdateModel(GameObject unit, in int[] equipedItems, in int prevItem = 0)
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        if (0 != prevItem)
        {
            int[] index = _modelItemPoint[_itemList.ItemSearch(prevItem).Name];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(false);
        }

        for (int i = 0; i < 2; ++i)
        {
            string itemName = _itemList.ItemSearch(equipedItems[i]).Name;

            int[] index = _modelItemPoint?[itemName];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(true);
        }
    }

    #region Private Variable

    // 아이템 이름 > 오브젝트에서의 아이템 위치
    private static Dictionary<string, int[]> _modelItemPoint = new Dictionary<string, int[]>();


    private static ItemList _itemList;
    #endregion

    #region Private Function

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        const int _completeModelCount = 6;
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
        itemNameList[itemIndex++] = "A.I의 머리 파츠";
        itemNameList[itemIndex++] = "A.I의 몸통 파츠";
        itemNameList[itemIndex++] = "제국의 헬멧";
        itemNameList[itemIndex++] = "제국의 슈트";

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
            v[1] = 1 - (i % equipmentCount);

            _modelItemPoint.Add(itemNameList[i], v);
        }
    }

    #endregion
}
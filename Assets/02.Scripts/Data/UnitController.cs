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
        if (_aniPro == null)
            _aniPro = transform.GetChild(0).GetComponent<AnimatorPro>();

        _aniPro.Init(transform.GetChild(0).GetComponent<Animator>());

        _status.UpdateItems();
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

    #region Show Inspector

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

    //능력 번후
    [HideInInspector]
    private int _abilityNum;

    // 공격 범위일듯.
    // public SphereCollider _attackAreaCollider;

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
    public FieldObject _enemyCastleObject;

    #endregion

    #region Hide Inspector

    #region Macro

    private static readonly int _idAttack = Animator.StringToHash("Attack");
    private static readonly int _idAttackSpd = Animator.StringToHash("AttackSpeed");
    private static readonly int _idMove = Animator.StringToHash("Move");

    #endregion

    #region 유닛 상태

    public UnitStatus _status;

    //방어력
    // public float _defensivePower { get { return _status._defensivePower; } }

    //공격 데미지
    private float _attackDamage { get { return _status._attackDamage; } }

    //공격 속도
    private float _attackSpeed { get { return _status._attackSpeed; } }

    //공격 범위
    private float _attackRange { get { return _status._attackRange; } }

    //이동 속도
    private float _moveSpeed { get { return _status._moveSpeed; } }

    // 유닛 코스트
    public int _cost { get { return _status._cost; } }

    // second
    public float _coolTime { get { return _status._coolTime; } }

    #endregion

    #region Other

    //애니메이터 관련 변수
    //private Animator _animator;
    private AnimatorPro _aniPro;

    //리지드 바디 변수
    //private Rigidbody _rig;

    //유닛 상태에 대한 변수
    public eState _curState = eState.NONE;

    //필드 관점에서의 해당 유닛의 상태
    public FieldObject _curTarget = null;

    //이동에 대한 파라미터
    // private float moveParameter;

    // bool _isSpawn = false;
    #endregion

    #endregion

    #region Monobehaviour Function

    private void FixedUpdate()
    {
        //if (!_isSpawn) return;

        //현재 상태가 비어 있거나, 죽었다면 : return
        if (_isDead) return;
        
        //이동 <--> 공격, 상태 변환
        // UpdateState();

        //상태 변수를 통한, 유닛 업데이트
        UpdateUnit();
    }

    private void OnEnable()
    {
        UpdateTarget();

        //_navMeshAgent.SetDestination(_curTarget.transform.position);
        _navMeshAgent.updateRotation = false;

        _aniPro.SetParam(_idAttackSpd, _attackSpeed + .15f);
        _aniPro.SetParam(_idAttack, false);

        _curState = eState.IDLE;

        _team = _status._team;
        _curHp = _status._curhealth;
        _maxHp = _status._maxhealth;

        _navMeshAgent.stoppingDistance = _status._attackRange;
        _navMeshAgent.speed = _status._moveSpeed;

        //_isSpawn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //닿은 생대가 유닛이 아니라면 : 아래 코드 구문 실행 X
        if (!other.CompareTag("Unit") || other.isTrigger)
            return;

        //other의 필드 관점의 데이터를 가져옴.
        var target = other.GetComponent<FieldObject>();

        //공격할 타겟 큐에 해당 타겟이 있거나, 타겟의 팀이 우리 팀이라면,
        if (_attackTargets.Contains(target) || target._team == _team)
            return;

        // if(_team == eTeam.PLAYER) { Debug.Log("Enemy Check !"); }

        //공격 타겟에 해당 타겟을 넣어줍니다.
        _attackTargets.Enqueue(target);
    }

    #endregion

    #region Private Function

    private void UpdateUnit()
    {
        //moveParameter = 0;

        UpdateMonobehaviour();

        UpdateAnimation();
    }

    private void UpdateMonobehaviour()
    {
        if (_curTarget == _enemyCastleObject || _curTarget.GetCurHealth() <= 0)
            UpdateTarget();

        float remainingDistance = (transform.position - _curTarget.transform.position).magnitude;

        if(_team == eTeam.PLAYER) { Debug.Log("" + remainingDistance + "   " + _attackRange); }

        if (remainingDistance <= _attackRange)
        {
            _curState = eState.ATTACK;
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;

            if (_attackTime <= _attackSpeed) { _attackTime += Time.deltaTime; return; }

            _attackTime = 0f;

            //데미지 리시브
            _curTarget.DamageReceive(_attackDamage);

            // if(_team == eTeam.PLAYER) { Debug.Log("Hit !"); }

            if (_curTarget.GetCurHealth() <= 0)
                UpdateTarget();
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _curState = eState.MOVE;
        }
    }

    private void UpdateAnimation()
    {
        if (_aniPro == null) return;
        
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

        if (_curState == eState.ATTACK)
        {
            _aniPro.SetParam(_idAttack, true);
        }
        else if (_curState == eState.MOVE) 
        {
            _aniPro.SetParam(_idAttack, false);

            _aniPro.SetParam(_idMove, 1.0f);
        }
    }

    void UpdateTarget()
    {
        if (_attackTargets.Count == 0)
        {
            _curTarget = _enemyCastleObject;
        }
        else
        {
            _curTarget = _attackTargets.Dequeue();
        }

        _navMeshAgent.SetDestination(_curTarget.transform.position);
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

        int[] index;
        if (0 != prevItem)
        {
            index = _modelItemPoint[_itemList.ItemSearch(prevItem).Name];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(false);
        }

        for (int i = 0; i < 2; ++i)
        {
            string itemName = _itemList.ItemSearch(equipedItems[i]).Name;

            index = _modelItemPoint?[itemName];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(true);
        }

        if (equipedItems[2] != 0)
        {
            string leftItemName = _itemList.ItemSearch(equipedItems[2]).Name;

            index = _modelItemPoint?[leftItemName];

            unit.transform.GetChild(unit.transform.childCount - 3).GetChild(index[1]).gameObject.SetActive(true);
        }

        if (equipedItems[3] != 0)
        {
            string rightItemName = _itemList.ItemSearch(equipedItems[3]).Name;

            index = _modelItemPoint?[rightItemName];

            unit.transform.GetChild(unit.transform.childCount - 2).GetChild(index[1]).gameObject.SetActive(true);
        }
    }

    #region Private Variable

    // 아이템 이름 > 오브젝트에서의 아이템 위치
    private static Dictionary<string, int[]> _modelItemPoint = new Dictionary<string, int[]>();


    private static ItemList _itemList;
    #endregion

    #region Private Function

    private static void InitData(ref string[] armourList, ref int armourIndex, ref string[] weaponList, ref int weaponIndex)
    {
        armourList[armourIndex++] = "일반 머리";
        armourList[armourIndex++] = "일반 옷";
        armourList[armourIndex++] = "견습 기사의 투구";
        armourList[armourIndex++] = "견습 기사의 갑옷";
        armourList[armourIndex++] = "셔우드 숲의 모자";
        armourList[armourIndex++] = "셔우드 숲의 코트";
        armourList[armourIndex++] = "하얀 눈의 모자";
        armourList[armourIndex++] = "하얀 눈의 옷";
        armourList[armourIndex++] = "A.I의 머리 파츠";
        armourList[armourIndex++] = "A.I의 몸통 파츠";
        armourList[armourIndex++] = "제국의 헬멧";
        armourList[armourIndex++] = "제국의 슈트";

        weaponList[weaponIndex++] = "ㅡ"; // +창
        weaponList[weaponIndex++] = "병사의 창";
        weaponList[weaponIndex++] = "십자창";
        weaponList[weaponIndex++] = "기사의 검";
        weaponList[weaponIndex++] = "병사의 검";
        weaponList[weaponIndex++] = "ㅡㅡ"; // 소주
        weaponList[weaponIndex++] = "사각 나무 방패";
        weaponList[weaponIndex++] = "셔우드의 활";
    }

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        const int _completeArmourCount = 6;
        string[] armourList = new string[_completeArmourCount * 2];
        int armourIndex = 0;

        const int _completeWeaponCount = 6;
        string[] weaponList = new string[_completeWeaponCount * 2];
        int weaponIndex = 0;

        InitData(ref armourList, ref armourIndex, ref weaponList, ref weaponIndex);

        int equipmentCount = 2;
        int[] modelNumList = new int[_completeArmourCount * 2];
        for (int i = 1; i < _completeArmourCount * 2 + 1; ++i)
        {
            modelNumList[i - 1] = (1 == i % 2 ? i / 2 : i / 2 - 1);
        }

        int[] v;
        for (int i = 0; i < armourIndex; ++i)
        {
            v = new int[equipmentCount];
            v[0] = modelNumList[i];
            v[1] = 1 - (i % equipmentCount);

            _modelItemPoint.Add(armourList[i], v);
        }

        for (int i = 0; i < weaponIndex; ++i)
        {
            v = new int[equipmentCount];
            v[0] = 0;
            v[1] = i;

            _modelItemPoint.Add(weaponList[i], v);
        }
    }

    #endregion
}
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

        _navMeshAgent.updateRotation = false;

        _status.UpdateItems();
        _curState = eState.IDLE;

        _aniPro.SetParam(_idAttackSpd, _attackSpeed);
        _aniPro.SetParam(_idAttack, false);

        UpdateTarget();
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

        _team = _status._team;
        _curHp = _status._curhealth;
        _maxHp = _status._maxhealth;

        _navMeshAgent.stoppingDistance = _status._attackRange;
        _navMeshAgent.speed = _status._moveSpeed;

        UnitAnimationManager.Update(_status._equipedItems[2], _status._equipedItems[3], _aniPro);
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
        UpdateMonobehaviour();

        UpdateAnimation();
    }

    private void UpdateMonobehaviour()
    {
        if (_curTarget == _enemyCastleObject || _curTarget.GetCurHealth() <= 0)
            UpdateTarget();

        float remainingDistance = (transform.position - _curTarget.transform.position).magnitude; // _navMeshAgent.remainingDistance; //

        if (remainingDistance <= _attackRange)
        {
            _curState = eState.ATTACK;
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;

            if (_attackTime <= _attackSpeed) { _attackTime += Time.deltaTime; return; }

            _attackTime = 0f;

            //데미지 리시브
            _curTarget.DamageReceive(_attackDamage);

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
        else
        {
            transform.LookAt(_curTarget.transform);
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

        if (gameObject.activeSelf)
            _navMeshAgent.SetDestination(_curTarget.transform.position);
    }

    #endregion
}

public class UnitModelManager
{
    public static void Reset(GameObject unit, in int[] equipedItems)
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        _weaponFindPoint = unit.transform.childCount;

        Transform leftWeaponTrs = unit.transform.GetChild(_leftWeaponPoint);
        Transform rightWeaponTrs = unit.transform.GetChild(_rightWeaponPoint);

        // 방어구 장착 해제
        for (int i = 0; i < 2; ++i)
        {
            if(0 == equipedItems[i]) { continue; }
            string itemName = _itemList.ItemSearch(equipedItems[i]).Name;

            int[] index = _modelItemPoint?[itemName];

            unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(false);
        }

        // 왼손무기 장착 해제
        if (equipedItems[2] != 0)
        {
            string leftItemName = _itemList.ItemSearch(equipedItems[2]).Name;

            int[] index = _modelItemPoint?[leftItemName];

            leftWeaponTrs.GetChild(index[1]).gameObject.SetActive(false);
        }

        // 오른손 무기 장착 해제
        if (equipedItems[3] != 0)
        {
            string rightItemName = _itemList.ItemSearch(equipedItems[3]).Name;

            int[] index = _modelItemPoint?[rightItemName];

            rightWeaponTrs.GetChild(index[1]).gameObject.SetActive(false);
        }
    }

    public static void Update(GameObject unit, in int[] equipedItems,int prevItem = 0)
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        _weaponFindPoint = unit.transform.childCount;

        Transform leftWeaponTrs = unit.transform.GetChild(_leftWeaponPoint);
        Transform rightWeaponTrs = unit.transform.GetChild(_rightWeaponPoint);

        for (int i = 0; i < equipedItems.Length; ++i) 
        {
            if(equipedItems[i] == prevItem)
            {
                prevItem = 0;
                break;
            }
        }

        int[] index;
        // 이전무기 장착해제
        if (0 != prevItem)
        {
            GameItem.Item i = _itemList.ItemSearch(prevItem);
            index = _modelItemPoint[i.Name];

            // true 면 무기 false 면 방어구
            if (i.Type > GameItem.eItemType.WEAPONS)
            {
                if (leftWeaponTrs.GetChild(index[1]).gameObject.activeSelf)
                    leftWeaponTrs.GetChild(index[1]).gameObject.SetActive(false);

                if (rightWeaponTrs.GetChild(index[1]).gameObject.activeSelf)
                    rightWeaponTrs.GetChild(index[1]).gameObject.SetActive(false);
            }
            else
            {
                if (unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.activeSelf)
                    unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(false);
            }
        }

        // 방어구 장착
        for (int i = 0; i < 2; ++i)
        {
            string itemName = _itemList.ItemSearch(equipedItems[i]).Name;

            index = _modelItemPoint?[itemName];

            if (!unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.activeSelf)
                unit.transform.GetChild(index[0]).GetChild(index[1]).gameObject.SetActive(true);
        }

        // 왼손무기 장착
        if (equipedItems[2] != 0)
        {
            string leftItemName = _itemList.ItemSearch(equipedItems[2]).Name;

            index = _modelItemPoint?[leftItemName];

            leftWeaponTrs.GetChild(index[1]).gameObject.SetActive(true);
        }

        // 오른손 무기 장착
        if (equipedItems[3] != 0)
        {
            string rightItemName = _itemList.ItemSearch(equipedItems[3]).Name;

            index = _modelItemPoint?[rightItemName];

            rightWeaponTrs.GetChild(index[1]).gameObject.SetActive(true);
        }
    }

    #region Variable

    // 아이템 이름 > 오브젝트에서의 아이템 위치
    private static Dictionary<string, int[]> _modelItemPoint = new Dictionary<string, int[]>();

    private static ItemList _itemList;

    private static int _weaponFindPoint = 0;
    private static int _leftWeaponPoint { get { return _weaponFindPoint - 3; } }
    private static int _rightWeaponPoint { get { return _weaponFindPoint - 2; } }
    #endregion

    #region Private Function

    private static void InitData(ref List<string> armourList, ref List<string> weaponList)
    {
        GameItem.eCodeType helmet = GameItem.eCodeType.HELMET;
        GameItem.eCodeType armour = GameItem.eCodeType.BODYARMOUR;

        for (int i = 0; i < _itemList.GetCodeItemCount(helmet); ++i)
        {
            int code = _itemList.CodeSearch(helmet, i);

            armourList.Add(_itemList.ItemSearch(code).Name);

            code = _itemList.CodeSearch(armour, i);

            armourList.Add(_itemList.ItemSearch(code).Name);
        }

        GameItem.eCodeType weapon = GameItem.eCodeType.WEAPON;
        for (int i = 0; i < _itemList.GetCodeItemCount(weapon); ++i)
        {
            int code = _itemList.CodeSearch(weapon, i);

            weaponList.Add(_itemList.ItemSearch(code).Name);
        }
    }

    private static void Init() 
    {
        _itemList = Manager.Get<GameManager>().itemList;

        List<string> armourList = new List<string>();

        List<string> weaponList = new List<string>();

        InitData(ref armourList, ref weaponList);

        int equipmentCount = 2;

        int[] v;
        for (int i = 0; i < armourList.Count; ++i)
        {
            v = new int[equipmentCount];
            v[0] = (1 == (i + 1) % 2 ? (i + 1) / 2 : (i + 1) / 2 - 1);
            v[1] = 1 - (i % equipmentCount);

            _modelItemPoint.Add(armourList[i], v);
        }

        for (int i = 0; i < weaponList.Count; ++i)
        {
            v = new int[equipmentCount];
            v[0] = 0;
            v[1] = i;

            _modelItemPoint.Add(weaponList[i], v);
        }
    }

    #endregion
}

public class UnitIconManager
{
    public static void Reset(GameObject IconObject)
    {

    }
    public static void Update(GameObject IconObject, int headItemNum)
    {
        if (0 == _iconPoints.Count)
            Init();

        if (!IconObject) return;

        GameItem.Item headItem = null;
        if ((headItem = _itemList.ItemSearch(headItemNum)) == null) return;

        if (!_iconPoints.ContainsKey(headItem.Name)) return;

        int iconPoint = _iconPoints[headItem.Name];

        GameObject headObject = null;
        if ((headObject = IconObject.transform.GetChild(iconPoint).gameObject) == null) { return; }

        headObject.SetActive(true);
    }
    
    #region Variable

    static Dictionary<string, int> _iconPoints = new Dictionary<string, int>();

    private static ItemList _itemList;

    #endregion

    #region Private Function
    private static void InitData(ref List<string> iconNames)
    {
        GameItem.eCodeType helmet = GameItem.eCodeType.HELMET;
        for (int i = 0; i < _itemList.GetCodeItemCount(helmet); ++i)
        {
            int code = _itemList.CodeSearch(helmet, i);

            iconNames.Add(_itemList.ItemSearch(code).Name);
        }

        Debug.Log("");
    }

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        List<string> iconNames = new List<string>();

        InitData(ref iconNames);

        for (int i = 0; i < iconNames.Count; ++i)
        {
            _iconPoints.Add(iconNames[i], i);
        }
    } 
    #endregion
}

public class UnitAnimationManager
{
    public static void Update(int leftWeaponCode, int rightWeaponCode, Animator ani) 
    {
        int num = -1;

        FindNum(leftWeaponCode, rightWeaponCode,ref num);

        if (num != -1)
            ani.SetInteger(_idWeaponType, num);
    }

    public static void Update(int leftWeaponCode, int rightWeaponCode, AnimatorPro ani) 
    {
        int num = -1;

        FindNum(leftWeaponCode, rightWeaponCode,ref num);

        if (num != -1)
            ani.SetParam(_idWeaponType, num);
    }
    
    #region Variable

    static Dictionary<GameItem.eItemType, string> _typeStrings = new Dictionary<GameItem.eItemType, string>();
    static Dictionary<string, int> _typeAnimationNum = new Dictionary<string, int>();

    private static ItemList _itemList;

    private static readonly int _idWeaponType = Animator.StringToHash("WeaponType");
    #endregion

    #region Private Function

    private static void FindNum(int leftWeaponCode,int rightWeaponCode,ref int num)
    {
        if (_typeStrings.Count == 0)
            Init();

        GameItem.Item leftWeapon = _itemList.ItemSearch(leftWeaponCode);
        GameItem.Item rightWeapon = _itemList.ItemSearch(rightWeaponCode);

        string leftString = "", rightString = "";
        if (leftWeapon != null && _typeStrings.ContainsKey(leftWeapon.Type))    leftString = _typeStrings[leftWeapon.Type];
        if (rightWeapon != null && _typeStrings.ContainsKey(rightWeapon.Type))  rightString = _typeStrings[rightWeapon.Type];

        if (!_typeAnimationNum.ContainsKey(leftString + rightString) &&
           !_typeAnimationNum.ContainsKey(rightString + leftString)) { leftString = rightString = ""; }

        num = _typeAnimationNum.ContainsKey(leftString + rightString) ?
            _typeAnimationNum[leftString + rightString] :
            _typeAnimationNum[rightString + leftString];
    }

    private static void InitData(ref List<string> aniName)
    {
        aniName.Add("Bow");
        aniName.Add("SwordShield");
        aniName.Add("Spear");
        aniName.Add("SwordSword");
        aniName.Add("Sword");
        aniName.Add("SpearShield");
        aniName.Add("SwordSpear");

        _typeAnimationNum.Add("", 5); // 아무 무기도 없을때 애니메이션은 Sword(제일 무난..)
    }

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        List<string> typeName = new List<string>();

        for (int i = 1; i < GameItem.eItemType.LAST - GameItem.eItemType.WEAPONS; ++i)
        {
            typeName.Add((GameItem.eItemType.WEAPONS + i).ToString());
        }

        List<string> aniName = new List<string>();

        InitData(ref aniName);

        GameItem.eItemType t = GameItem.eItemType.WEAPONS;
        for (int i = 0; i < typeName.Count; ++i)
        {
            _typeStrings.Add(t + i + 1, typeName[i]);
        }

        _typeStrings.Remove(GameItem.eItemType.Hammer);

        for (int i = 0; i < aniName.Count; ++i)
        {
            _typeAnimationNum.Add(aniName[i], i + 1);
        }
    } 
    #endregion
}
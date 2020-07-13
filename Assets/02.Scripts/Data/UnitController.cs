using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Text;

#region Public Enum

public enum eTeam
{
    PLAYER,
    ENEMY
}

public enum eAni
{
    NONE,
    IDLE,
    MOVE,
    ATTACK,
}

#endregion

public class UnitController : FieldObject
{
    #region Public Function
    #region Unit Reset

    public void Spawn()
    { // Spawn() -> OnEnable() 순서

        // Inspector 에서 드래그드롭 해줘야 할 오브젝트들
        #region Null Check

        if (_ani == null) { _ani = GetComponentInChildren<UnitAnimation>(); LogMessage.Log("UnitCtrl : AniPro is Null"); }
        if (_IsNavMeshAgent) { }
        if (_eye == null) { _eye = GetComponentInChildren<UnitEye>(); LogMessage.Log("UnitCtrl : Eye is Null"); }
        #endregion

        #region AI Awake

        // 나머지 데이터들 Init
        _navMeshAgent.updateRotation = false;

        #endregion

        #region Status Awake
        for (int i = 0; i < 4; ++i)
        {
            if (_status._abilities[i] == null) { continue; }

            Ability[i].Enable();
        }

        ++_status._attackRange;

        CurState = eAni.IDLE;

        _isDead = false;
        #endregion
    }

    #endregion

    #region Unit Interaction

    public override void DamageReceive(float statDamage, FieldObject receiveObject)
    #region Function Content
    {
        for (int i = 0; i < 4; ++i)
        {
            if (_status._abilities[i] == null) { continue; }

            Ability[i].Beaten(_curTarget);
        }

        //데미지를 받습니다.
        float damage = Mathf.Max(statDamage - (_status._defensivePower - receiveObject.DefensiveCleavage), 0);

        UnitController uc = receiveObject.GetComponent<UnitController>();
        
        if (uc != null)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (uc.Ability[i] == null) { continue; }

                uc.Ability[i].Hit(ref damage);
            }
        }

        CurHealth -= damage;

        _healthBarImage.fillAmount = RemainHealth;

        //체력이 0 초과라면 : 아래 코드 구문 실행 x
        if (CurHealth > 0) return;

        //사망 처리 한다.
        _isDead = true;

        _healthBarObject.SetActive(false);

        //해당 유닛과 체력바를 삭제 목록에 올립니다.
        DeleteObjectSystem.AddDeleteObject(gameObject);
    }
    #endregion

    public void Heal(float heal)
    #region Function Content
    {
        CurHealth = Mathf.Min(CurHealth + heal, MaxHealth);
    }
    #endregion

    #endregion

    #region Unit Animation Action

    public void OnEffect()
    {
        if (particle == null) return;
        
        if (!particle.gameObject.activeSelf)
            particle.gameObject.SetActive(true);

        particle.Play();
    }

    public void AttackRight()
    {
        if (RightAttackDamage == 0 || _isTest || CurState != eAni.ATTACK) { return; }

        float remainingDistance = (_curTarget.transform.position - transform.position).magnitude;
        if (remainingDistance > _navMeshAgent.stoppingDistance * 2) { return; }

        float curDamage = RightAttackDamage;

        int weaponIndex = (int)GameItem.eCodeType.RightWeapon;
        ItemAbility abil = _status._abilities[weaponIndex];
        if (abil != null)
            abil.Attack(_curTarget, ref curDamage);

        _curTarget.DamageReceive(curDamage, this);

        if (_curTarget.CurHealth <= 0)
            _eye.UpdateTarget();
    }

    public void AttackLeft()
    {
        if (LeftAttackDamage == 0 || _isTest || CurState != eAni.ATTACK) { return; }

        float remainingDistance = (_curTarget.transform.position - transform.position).magnitude;
        if (remainingDistance > _navMeshAgent.stoppingDistance * 2) { return; }

        float curDamage = LeftAttackDamage;

        int weaponIndex = (int)GameItem.eCodeType.LeftWeapon;
        ItemAbility abil = _status._abilities[weaponIndex];
        if (abil != null)
            abil.Attack(_curTarget, ref curDamage);

        _curTarget.DamageReceive(curDamage, this);

        if (_curTarget.CurHealth <= 0)
            _eye.UpdateTarget();
    }
    #endregion

    #region Unit Eye

    public void UpdateTarget()
    {
        FieldObject newTarget = _eye.CurTarget == null ? _enemyCastleObject : _eye.CurTarget;

        if (newTarget == _curTarget) { return; }

        {   // 타겟을 바꾸는 경우
            _curTarget = newTarget;

            _navMeshAgent.SetDestination(_curTarget.transform.position);

            _navMeshAgent.stoppingDistance = AttackRange;
        }
    }
    #endregion 

    #endregion

    #region Variable

    #region Inspector

    public bool _isTest = false;

    // 이 유닛의 애니메이션
    [SerializeField]
    private UnitAnimation _ani;

    public Canvas _canvas;

    private RectTransform _canvasRectTrs;
    private Camera _hpCamera;

    // 이 유닛의 AI 

    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;
    private bool _IsNavMeshAgent
    {
        get
        {
            if (_navMeshAgent == null)
            {
                _navMeshAgent = GetComponent<NavMeshAgent>(); LogMessage.Log("UnitCtrl  NavAgent is Null");

                if (_navMeshAgent == null) { return false; }
            }

            return true;
        }
    }

    // 이 유닛의 눈
    public UnitEye _eye;

    [SerializeField]
    private GameObject effectObject;

    #endregion

    // 아이템 능력 번호
    [HideInInspector]
    private int[] _abilityNum = new int[4];

    // 상대방 진영의 성
    public FieldObject _enemyCastleObject;

    //필드 관점에서의 해당 유닛의 상태
    private FieldObject _curTarget = null;

    public ParticleSystem particle;

    #region 유닛 상태

    public GameObject _healthBarObject;
    public Image _healthBarImage;

    //유닛 상태에 대한 변수
    public eAni _curState = eAni.NONE;
    public eAni CurState
    {
        get { return _curState; }
        set
        {
            if (_curState != value && gameObject.activeSelf)
            {
                switch (value)
                {
                    case eAni.IDLE: _navMeshAgent.isStopped = true; break;
                    case eAni.MOVE:
                        _navMeshAgent.isStopped = false;
                        if (_curState == eAni.IDLE && !_eye._isEnemy) { _eye.UpdateTarget(); }
                        break;
                    case eAni.ATTACK: _navMeshAgent.isStopped = true; break;
                }

                _ani.UpdateAni(value);
            }

            _curState = value;
        }
    }

    public UnitStatus _status;

    public override float   CurHealth         { get => _status._curhealth; set => _status._curhealth = value; }
    public override float   MaxHealth         { get => _status._maxhealth; set => _status._maxhealth = value; }
    public override float   DefencePower      { get => _status._defensivePower; set => _status._defensivePower = value; }
    public float            AttackDamage      { get => _status.AttackDamage; }
    public float            LeftAttackDamage  { get => _status.LeftAttackDamage; }
    public float            RightAttackDamage { get => _status.RightAttackDamage; }
    public override float   AttackSpeed       { get { return _status.AttackSpeed; } set { _status._attackSpeed = value; } }
    public float            AttackRange       => _status._attackRange;
    public override float   MoveSpeed         { get { return _status._moveSpeed; } set { _status._moveSpeed = value; } }
    public int              Cost              => _status._cost;
    public float            CoolTime          => _status._coolTime;
    public ItemAbility[]    Ability           => _status.Abilities;



    #endregion

    #endregion

    #region Monobehaviour Function

    void Awake()
    {
        if (_isTest)
        {
            _status = new UnitStatus();
            _status.Init();
        }
    }

    override protected void OnEnable()
    { // Spawn() -> OnEnable() 순서
        base.OnEnable();

        if (_isTest) { return; }

        #region UI Enable

        if (!_healthBarObject.activeSelf)
            _healthBarObject.SetActive(true);

        _healthBarImage = _healthBarObject.transform.GetChild(0).GetComponent<Image>();

        int childCount = _healthBarObject.transform.childCount;
        _stateSpriteUIs = new Image[childCount];
        for (int i = 1; i < childCount; ++i)
            _stateSpriteUIs[i - 1] = _healthBarObject.transform.GetChild(i).GetComponent<Image>();

        _healthBarImage.fillAmount = RemainHealth;

        _canvasRectTrs = _canvas.GetComponent<RectTransform>();
        _hpCamera = _canvas.worldCamera;


        #endregion

        #region AI Enable
        _team = _status._team;

        _navMeshAgent.stoppingDistance = Mathf.Max(1.8f, AttackRange);
        _navMeshAgent.speed = MoveSpeed;

        _curTarget = _enemyCastleObject;
        _navMeshAgent.SetDestination(_curTarget.transform.position);
        #endregion

        #region Status Enable

        for (int i = 0; i < 4; ++i)
        {
            if (_status._abilities[i] == null) { continue; }

            Ability[i].Start(this);
        }

        #endregion

        #region Effect Enable

        UnitEffectManager.Update(_status._equipedItems[2], _status._equipedItems[3], ref particle, ref effectObject);

        if (particle != null)
        {
            particle.playbackSpeed = 1 * AttackSpeed;
            for (int i = 0; i < particle.transform.childCount; ++i)
            {
                particle.transform.GetChild(i).GetComponent<ParticleSystem>().playbackSpeed = 1 * AttackSpeed;
            }
        }
        #endregion
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        //현재 상태가 비어 있거나, 죽었다면 : return
        if (_isDead) { CurState = eAni.IDLE; return; }

        //상태 변수를 통한, 유닛 업데이트
        UpdateUnit();
    }

    private void LateUpdate()
    {
        if(_canvasRectTrs == null && _hpCamera == null) { return; }

        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTrs, screenPos, _hpCamera, out localPos);

        localPos.x += 3;
        localPos.y += 80;

        _healthBarObject.transform.localPosition = localPos;
    }

    #endregion

    #region Private Function

    private void UpdateUnit()
    {
        if (_navMeshAgent.pathPending || !gameObject.activeSelf) { return; }

        UpdateMonobehaviour();

    }

    private void UpdateMonobehaviour()
    {
        if (!_IsNavMeshAgent || _curTarget == null) { return; }

        if (_navMeshAgent.desiredVelocity.sqrMagnitude >= .1f * .1f)
        {
            Vector3 direction = _navMeshAgent.desiredVelocity;

            Quaternion targetAngle = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  targetAngle,
                                                  Time.deltaTime * 8.0f);
        }

        if (_curTarget.CurHealth <= 0) _eye.UpdateTarget();

        float remainingDistance = (_curTarget.transform.position - transform.position).magnitude;

        if (remainingDistance <= _navMeshAgent.stoppingDistance * 2
            || (_navMeshAgent.velocity == Vector3.zero && remainingDistance <= _navMeshAgent.stoppingDistance * 2))
        {
            transform.LookAt(_curTarget.transform);

            CurState = eAni.ATTACK;
        }
        else
        {
            CurState = eAni.MOVE;
        }

        for (int i = 0; i < 4; ++i)
        {
            if (_status._abilities[i] == null) { continue; }

            Ability[i].Update(Time.fixedDeltaTime);
        }
    }

    #endregion
}

public class UnitModelManager
{
    public static bool Reset(GameObject unit, in int[] equipedItems)
    #region Function Content
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return false; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        _weaponFindPoint = unit.transform.childCount;

        Transform leftWeaponTrs = unit.transform.GetChild(_leftWeaponPoint);
        Transform rightWeaponTrs = unit.transform.GetChild(_rightWeaponPoint);

        // 방어구 장착 해제
        for (int i = 0; i < 2; ++i)
        {
            if (0 == equipedItems[i]) { continue; }
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

        return true;
    }
    #endregion

    public static void Reset(GameObject unit)
    #region Function Content
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        _weaponFindPoint = unit.transform.childCount;

        Transform leftWeaponTrs = unit.transform.GetChild(_leftWeaponPoint);
        Transform rightWeaponTrs = unit.transform.GetChild(_rightWeaponPoint);

        // 방어구 장착 해제
        for (int i = 0; i < _leftWeaponPoint - 1; ++i)
        {
            if(!unit.transform.GetChild(i).gameObject.activeSelf) { continue; }

            for (int j = 0; j < 2; ++j)
            {
                if (unit.transform.GetChild(i).GetChild(j).gameObject.activeSelf)
                    unit.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }

        GameObject curWeapons;
        for (int i = 0; i < leftWeaponTrs.childCount; ++i)
        {
            curWeapons = leftWeaponTrs.GetChild(i).gameObject;

            if (curWeapons.activeSelf)
                curWeapons.SetActive(false);
        }

        for (int i = 0; i < rightWeaponTrs.childCount; ++i)
        {
            curWeapons = rightWeaponTrs.GetChild(i).gameObject;

            if (curWeapons.activeSelf)
                curWeapons.SetActive(false);
        }
    }

    #endregion

    public static bool Update(GameObject unit, in int[] equipedItems, int prevItem = 0, int colorNum = 1)
    #region Function Content
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return false; }

        if (!unit.activeSelf) { unit.SetActive(true); }

        _weaponFindPoint = unit.transform.childCount;

        Transform leftWeaponTrs = unit.transform.GetChild(_leftWeaponPoint);
        Transform rightWeaponTrs = unit.transform.GetChild(_rightWeaponPoint);

        int[] index;
        // 이전무기 장착해제
        if (0 != prevItem)
        {
            GameItem.Item i = _itemList.ItemSearch(prevItem);
            index = _modelItemPoint[i.Name];

            // true 면 무기 false 면 방어구
            if (i.AniType >= GameItem.eItemType.Sword)
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

        return true;
    }

    #endregion

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
        GameItem.eCodeType helmet = GameItem.eCodeType.Helmet;
        GameItem.eCodeType armour = GameItem.eCodeType.Bodyarmour;

        for (int i = 0; i < _itemList.GetCodeItemCount(helmet); ++i)
        {
            int code = _itemList.CodeSearch(helmet, i);

            armourList.Add(_itemList.ItemSearch(code).Name);

            code = _itemList.CodeSearch(armour, i);

            armourList.Add(_itemList.ItemSearch(code).Name);
        }

        GameItem.eCodeType weapon = GameItem.eCodeType.Weapon;
        for (int i = 0; i < _itemList.GetCodeItemCount(weapon); ++i)
        {
            int code = _itemList.CodeSearch(weapon, i);

            weaponList.Add(_itemList.ItemSearch(code).Name);
        }
    }

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        List<string> weaponList = new List<string>();
        List<string> armourList = new List<string>();

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
    public static void Reset(GameObject iconObject)
    #region Function Content
    {
        if (_iconPoints.Count == 0)
            Init();

        if (!iconObject) return;

        GameObject curIcon;
        for (int i = 0; i < iconObject.transform.childCount; ++i)
        {
            curIcon = iconObject.transform.GetChild(i).gameObject;
            if (curIcon.activeSelf) curIcon.SetActive(false);
        }
    }
    #endregion

    public static void Update(GameObject iconObject, int ItemNum)
    #region Function Content
    {
        if (0 == _iconPoints.Count)
            Init();

        if (!iconObject) return;

        GameItem.Item headItem;
        if ((headItem = _itemList.ItemSearch(ItemNum)) == null) return;

        if (!_iconPoints.ContainsKey(headItem.Name)) return;

        int iconPoint = _iconPoints[headItem.Name];

        GameObject headObject;
        if ((headObject = iconObject.transform.GetChild(iconPoint).gameObject) == null) { return; }

        headObject.SetActive(true);
    }

    #endregion

    public static void Update(GameObject iconObject, string ItemName)
    #region Function Content
    {
        if (0 == _iconPoints.Count)
            Init();

        if (!iconObject) return;

        if (!_iconPoints.ContainsKey(ItemName)) return;

        int iconPoint = _iconPoints[ItemName];

        GameObject headObject;
        if ((headObject = iconObject.transform.GetChild(iconPoint).gameObject) == null) { return; }

        headObject.SetActive(true);
    }
    #endregion

    public static void SetColor(GameObject iconObject, Color color)
    #region Function Content
    {
        iconObject.GetComponent<Image>().color = color;

        for (int i = 0; i < iconObject.transform.childCount; ++i)
        {
            if (iconObject.transform.GetChild(i).gameObject.activeSelf)
            {
                iconObject.transform.GetChild(i).GetComponent<Image>().color = color;
                break;
            }
        }
    }
    #endregion

    #region Variable

    static Dictionary<string, int> _iconPoints = new Dictionary<string, int>();

    private static ItemList _itemList;

    #endregion

    #region Private Function
    private static void InitData(ref List<string> iconNames)
    {
        GameItem.eCodeType armour = GameItem.eCodeType.Bodyarmour;
        GameItem.eCodeType helmet = GameItem.eCodeType.Helmet;
        int count = _itemList.GetCodeItemCount(helmet);
        for (int i = 0; i < count; ++i)
        {
            int headcode = _itemList.CodeSearch(helmet, i);

            iconNames.Add(_itemList.ItemSearch(headcode).Name);

            int bodycode = _itemList.CodeSearch(armour, i);

            iconNames.Add(_itemList.ItemSearch(bodycode).Name);
        }

        count = _itemList.GetCodeItemCount(GameItem.eCodeType.Weapon);
        for (int i = 0; i < count; ++i)
        {
            int code = _itemList.CodeSearch(GameItem.eCodeType.Weapon, i);

            iconNames.Add(_itemList.ItemSearch(code).Name);
        }
    }

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        List<string> iconNames = new List<string>();

        InitData(ref iconNames);

        int j = 0;
        int armourCount = _itemList.GetCodeItemCount(GameItem.eCodeType.Helmet) * 2;
        for (int i = 0; i < armourCount; ++i)
        {
            if (i != 0 && i % 2 == 0) { ++j; }

            _iconPoints.Add(iconNames[i], j);
        }

        // 방어구는 머리 몸 2가지
        int weaponCount = _itemList.GetCodeItemCount(GameItem.eCodeType.Weapon);
        for (int i = 0; i < weaponCount; ++i)
        {
            _iconPoints.Add(iconNames[armourCount + i], i);
        }
    }
    #endregion
}

public class UnitAnimationManager
{
    public static bool Update(int leftWeaponCode, int rightWeaponCode, Animator ani)
    #region Function Content

    {
        int num = -1;

        bool returnVal = FindNum(leftWeaponCode, rightWeaponCode, ref num);

        if (num != -1)
            ani.SetInteger(_idWeaponType, num);

        return returnVal;
    }

    #endregion

    public static bool FindNum(int leftWeaponCode, int rightWeaponCode, ref int num)
    #region Function Content

    {
        if (_typeStrings.Count == 0)
            Init();

        GameItem.Item leftWeapon = _itemList.ItemSearch(leftWeaponCode);
        GameItem.Item rightWeapon = _itemList.ItemSearch(rightWeaponCode);

        string leftString = "", rightString = "";
        if (leftWeapon != null && _typeStrings.ContainsKey(leftWeapon.AniType)) leftString = _typeStrings[leftWeapon.AniType];
        if (rightWeapon != null && _typeStrings.ContainsKey(rightWeapon.AniType)) rightString = _typeStrings[rightWeapon.AniType];

        StringBuilder sb = new StringBuilder(leftString);
        sb.Append("&").Append(rightString);

        if (!_typeAnimationNum.ContainsKey(sb.ToString())) { return false; }

        num = _typeAnimationNum[sb.ToString()];

        return true;
    }

    #endregion

    #region Variable

    static Dictionary<GameItem.eItemType, string> _typeStrings = new Dictionary<GameItem.eItemType, string>();
    static Dictionary<string, int> _typeAnimationNum = new Dictionary<string, int>();

    private static ItemList _itemList;

    private static readonly int _idWeaponType = Animator.StringToHash("WeaponType");
    #endregion

    #region Private Function

    private static void InitData(ref List<string> aniName)
    {
        aniName.Add("Bow&");
        aniName.Add("Shield&Sword");
        aniName.Add("&Spear");
        aniName.Add("Sword&Sword");
        aniName.Add("&Sword");
        aniName.Add("Shield&Spear");
        aniName.Add("Sword&Spear");
        aniName.Add("Spear&Spear");
        aniName.Add("Shield&Shield");
        aniName.Add("&TwoHandSword");
        aniName.Add("TwoHandSword&TwoHandSword");
        aniName.Add("Spear&");
        aniName.Add("Sword&");
        aniName.Add("TwoHandSword&");
        aniName.Add("&Bow");
        aniName.Add("Spear&Shield");
        aniName.Add("Sword&Shield");
        aniName.Add("Spear&Sword");

        _typeAnimationNum.Add("&", 5); // 아무 무기도 없을때 애니메이션은 Sword(제일 무난..)
    }

    private static void Init()
    {
        _itemList = Manager.Get<GameManager>().itemList;

        List<string> typeName = new List<string>();

        int weaponCount = (GameItem.eItemType.LAST - GameItem.eItemType.Sword);
        for (int i = 0; i < weaponCount; ++i)
        {
            typeName.Add((GameItem.eItemType.Sword + i).ToString());
        }

        List<string> aniName = new List<string>();

        InitData(ref aniName);

        GameItem.eItemType t = GameItem.eItemType.Sword;
        for (int i = 0; i < typeName.Count; ++i)
        {
            _typeStrings.Add(t + i, typeName[i]);
        }

        for (int i = 0; i < aniName.Count; ++i)
        {
            _typeAnimationNum.Add(aniName[i], i + 1);
        }
    }
    #endregion
}

public class UnitEffectManager
{
    public static void Update(int leftWeaponCode, int rightWeaponCode, ref ParticleSystem ps, ref GameObject EffectObject)
    #region Function Content
    {
        if (EffectObject is null || leftWeaponCode + rightWeaponCode == 0) { return; }

        int n = 0;
        UnitAnimationManager.FindNum(leftWeaponCode, rightWeaponCode, ref n);
        --n;

        if (EffectObject.transform.childCount > n)
        {
            GameObject curObject = EffectObject.transform.GetChild(n).gameObject;
            
            ps = curObject.GetComponent<ParticleSystem>();
        }
    }
    #endregion
}
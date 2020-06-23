using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
// using UnityEngine.AnimatorPro;
using UnityEngine.AI;

using UnityEngine.ParticleSystemJobs;
using System.Collections;
using TMPro.EditorUtilities;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public enum eTeam
{
    PLAYER,
    ENEMY
}

public class UnitController : FieldObject
{
    public void Spawn()
    { // Spawn() -> OnEnable() 순서

        // Inspector 에서 드래그드롭 해줘야 할 오브젝트들
        if (_ani == null)    { _ani = GetComponentInChildren<UnitAnimation>(); LogMassage.Log("UnitCtrl : AniPro is Null"); }
        if (_IsNavMeshAgent) { } // { _navMeshAgent = GetComponent<NavMeshAgent>(); Debug.Log("UnitCtrl  NavAgent is Null"); }
        if (_eye == null)    { _eye = GetComponentInChildren<UnitEye>(); LogMassage.Log("UnitCtrl : Eye is Null"); }

        // 나머지 데이터들 Init
        _navMeshAgent.updateRotation = false;

        //_status.UpdateItems();
        ++_status._attackRange;

        CurState = eAni.IDLE;

        _curHp = _status._maxhealth;
        _maxHp = _status._maxhealth;
        _isDead = false;
    }

    public override void DamageReceive(float damage) 
    {
        //데미지를 받습니다.
        _curHp -= damage;

        _healthBarImage.fillAmount = RemainHealth;

        //체력이 0 초과라면 : 아래 코드 구문 실행 x
        if (_curHp > 0) return;

        //사망 처리 한다.
        _isDead = true;

        _healthBarObject.SetActive(false);

        //해당 유닛과 체력바를 삭제 목록에 올립니다.
        DeleteObjectSystem.AddDeleteObject(gameObject);
    }


    #region Variable

    #region Inspector

    [SerializeField]
    bool _isTest = false;

    // 이 유닛의 애니메이션
    [SerializeField]
    private UnitAnimation _ani;

    // 이 유닛의 AI 

    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;
    private bool _IsNavMeshAgent 
    {
        get
        {
            if (_navMeshAgent == null)
            {
                _navMeshAgent = GetComponent<NavMeshAgent>(); Debug.Log("UnitCtrl  NavAgent is Null");

                if (_navMeshAgent == null) { return false; }
            }

            return true;
        }
    }

    // 이 유닛의 눈
    [SerializeField]
    private UnitEye _eye;

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

    #region 유닛 상태

    #region Enum

    public enum eAni
    {
        NONE,
        IDLE,
        MOVE,
        ATTACK,
    }

    #endregion

    public GameObject _healthBarObject;
    public Image _healthBarImage;

    //유닛 상태에 대한 변수
    public eAni _curState = eAni.NONE;
    public eAni CurState
    {
        get { return _curState; }
        set
        {
            if(_curState != value && gameObject.activeSelf)
            {
                if (!_navMeshAgent.pathPending)
                {
                    switch (value)
                    {
                        case eAni.IDLE: _navMeshAgent.isStopped = true; break;
                        case eAni.MOVE: _navMeshAgent.isStopped = false;
                            if (_curState == eAni.IDLE && !_eye._isEnemy) { _eye.UpdateTarget(); }
                            break;
                        case eAni.ATTACK: _navMeshAgent.isStopped = true; break;
                    }
                }

                _ani.Update(value);
            }

            _curState = value;
        }
    }

    public UnitStatus _status;

    //공격 데미지
    public float _attackDamage          { get { return _status._attackDamage; } }
    public float _leftAttackDamage      { get { return _status._leftAttackDamage; } }
    public float _rightAttackDamage     { get { return _status._rightAttackDamage; } }

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

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        if(_isTest)
        {
            _status = new UnitStatus();
            _status.Init();

        }
    }

    private void FixedUpdate()
    {
        //현재 상태가 비어 있거나, 죽었다면 : return
        if (_isDead) { CurState = eAni.IDLE; return; }
        
        //상태 변수를 통한, 유닛 업데이트
        UpdateUnit();
    }

    public Canvas _canvas;

    private RectTransform _canvasRectTrs;
    private Camera _hpCamera;

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTrs, screenPos, _hpCamera, out localPos);

        localPos.x += 3;
        localPos.y += 80;

        _healthBarObject.transform.localPosition = localPos;
    }

    private void OnEnable()
    { // Spawn() -> OnEnable() 순서
        if (!_healthBarObject.activeSelf)
            _healthBarObject.SetActive(true);

        _healthBarImage = _healthBarObject.transform.GetChild(0).GetComponent<Image>();
        _healthBarImage.fillAmount = RemainHealth;

        _team = _status._team;

        _navMeshAgent.stoppingDistance = _attackRange >= 3 ? _attackRange : 3;
        _navMeshAgent.speed = _moveSpeed;

        _curTarget = _enemyCastleObject;
        _navMeshAgent.SetDestination(_curTarget.transform.position);

        _canvasRectTrs = _canvas.GetComponent<RectTransform>();
        _hpCamera = _canvas.worldCamera;

        UnitEffectManager.Update(_status._equipedItems[2], _status._equipedItems[3], ref particle, ref effectObject);

        particle.playbackSpeed = 1 * _attackSpeed;
        for (int i = 0; i < particle.transform.childCount; ++i)
        {
            particle.transform.GetChild(i).GetComponent<ParticleSystem>().playbackSpeed = 1 * _attackSpeed;
        }
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
        if(!_IsNavMeshAgent) { return; }

        if (_navMeshAgent.desiredVelocity.sqrMagnitude >= .1f * .1f)
        {
            Vector3 direction = _navMeshAgent.desiredVelocity;

            Quaternion targetAngle = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  targetAngle,
                                                  Time.deltaTime * 8.0f);
        }

        float remainingDistance = (_curTarget.transform.position - transform.position).magnitude;

        if (_eye._isEnemy)
        {
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
        }
        else
        {
            if(remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                CurState = eAni.IDLE;
            }
            else
            {
                CurState = eAni.MOVE;
            }
        }
    }

    ParticleSystem particle;

    public void OnEffect()
    {
        particle.Play();
    }

    public void AttackRight()
    {
        if (_rightAttackDamage == 0 || _isTest || CurState != eAni.ATTACK) { return; }

        _curTarget.DamageReceive(_rightAttackDamage);

        if (_curTarget.CurHealth <= 0)
            _eye.UpdateTarget();
    }

    public void AttackLeft()
    {
        if(_leftAttackDamage == 0 || _isTest || CurState != eAni.ATTACK) { return; }

        _curTarget.DamageReceive(_leftAttackDamage);

        if (_curTarget.CurHealth <= 0)
            _eye.UpdateTarget();
    }

    public void UpdateTarget()
    {
        FieldObject newTarget = _eye.CurTarget == null ? _enemyCastleObject : _eye.CurTarget;

        if (newTarget == _curTarget) { return; }

        {   // 타겟을 바꾸는 경우
            _curTarget = newTarget;

            _navMeshAgent.SetDestination(_curTarget.transform.position);

            _navMeshAgent.stoppingDistance = _attackRange;
        }
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

    public static void Reset(GameObject unit)
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
            for (int j = 0; j < 2; ++j) 
            {
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

    public static void Update(GameObject unit, in int[] equipedItems, int prevItem = 0, int colorNum = 1)
    {
        if (0 == _modelItemPoint.Count)
            Init();

        if (null == unit) { return; }

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

    public static void Update(GameObject iconObject, int ItemNum)
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

    public static void Update(GameObject iconObject, string ItemName)
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

    public static void SetColor(GameObject iconObject, Color color)
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

    #region Variable

    static Dictionary<string, int> _iconPoints = new Dictionary<string, int>();

    private static ItemList _itemList;

    #endregion

    #region Private Function
    private static void InitData(ref List<string> iconNames)
    {
        GameItem.eCodeType helmet = GameItem.eCodeType.Helmet;
        GameItem.eCodeType armour = GameItem.eCodeType.Bodyarmour;
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
            if (i != 0 && i % 2 == 0)  { ++j; }

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
    public static void Update(int leftWeaponCode, int rightWeaponCode, Animator ani) 
    {
        int num = -1;

        FindNum(leftWeaponCode, rightWeaponCode,ref num);

        if (num != -1)
            ani.SetInteger(_idWeaponType, num);
    }

    public static void FindNum(int leftWeaponCode, int rightWeaponCode, ref int num)
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

        if (!_typeAnimationNum.ContainsKey(sb.ToString())) { num = _typeAnimationNum["&"]; return; }

        num = _typeAnimationNum[sb.ToString()];
    }

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
    public static void Update(int leftWeaponCode, int rightWeaponCode, ref ParticleSystem ps,ref GameObject EffectObject)
    {
        if(EffectObject is null || leftWeaponCode + rightWeaponCode == 0) { return; }
        
        int n = 0;
        UnitAnimationManager.FindNum(leftWeaponCode, rightWeaponCode, ref n);
        --n;

        if (EffectObject.transform.childCount > n)
        {
            GameObject curObject = EffectObject.transform.GetChild(n).gameObject;

            if (!curObject.activeSelf)
                curObject.SetActive(true);

            ps = curObject.GetComponent<ParticleSystem>();
        }
    }
}
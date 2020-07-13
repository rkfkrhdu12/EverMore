using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eCrowdControl
{
    FrostyFeet,
    Freezing,
    Burn,
    Poisoned,
    Inspire,
    Wither,
    DefensiveSpiral,

    Last
}

public class FieldObject : MonoBehaviour
{

    // 아군인지 적군인지에 대한 변수
    // [HideInInspector]
    public eTeam _team = eTeam.PLAYER;

    //죽었는지 살았는지에 대한 변수
    [HideInInspector]
    public bool _isDead;

    public virtual void DamageReceive(float damage, FieldObject receiveObject) { }

    public float _curHp;
    public float _maxHp;

    [HideInInspector] public float _defenseCleavage = 0.0f;

    [HideInInspector] public float DefaultCurHealth   = 0;            public virtual float CurHealth              { get { return _curHp; }        set { _curHp = value; } }
    [HideInInspector] public float DefaultMaxHealth   = 0;            public virtual float MaxHealth              { get { return _maxHp; }        set { _maxHp = value; } }
    [HideInInspector] public float DefaultDefencePower = 0;           public virtual float DefencePower           { get { return 0.0f; }          set { } }
    [HideInInspector] public float DefaultAttackSpeed = 0;            public virtual float AttackSpeed            { get { return 0.0f; }          set { } }
    [HideInInspector] public float DefaultMoveSpeed   = 0;            public virtual float MoveSpeed              { get { return 0.0f; }          set { } }
    [HideInInspector] public float DefaultDefensiveCleavage = 0;      public virtual float DefensiveCleavage      { get { return _defenseCleavage; }      set { _defenseCleavage = value; } }
    [HideInInspector] public float RemainHealth => CurHealth / MaxHealth;

    [HideInInspector] public Image[] _stateSpriteUIs;

    [HideInInspector] public List<string> _abilNameList = new List<string>();

    public bool IsDead { get => _isDead; }

    [HideInInspector] public bool[] _isCrowdControls = new bool[(int)eCrowdControl.Last];
    [HideInInspector] public float[] _CCTimes = new float[(int)eCrowdControl.Last];

    public delegate void ExitCC(eCrowdControl type, FieldObject obj);
    public ExitCC ExitCCFunction;

    public void DamageDot(float damage)
    {
        CurHealth -= damage;
    }

    public void CrowdControl(eCrowdControl curCC, float time)
    {
        _isCrowdControls[(int)curCC] = true;
        _CCTimes[(int)curCC] = time;
    }

    virtual protected void OnEnable()
    {
        DefaultCurHealth    = CurHealth;
        DefaultMaxHealth    = MaxHealth;
        DefaultDefencePower = DefencePower;
        DefaultAttackSpeed  = AttackSpeed;
        DefaultMoveSpeed    = MoveSpeed;
        DefaultDefensiveCleavage = DefensiveCleavage;
    }

    virtual protected void FixedUpdate()
    {
        for (int i = 0; i < (int)eCrowdControl.Last; ++i)
        {
            if (_isCrowdControls[i])
            {
                _CCTimes[i] = Mathf.Max(0, _CCTimes[i] - Time.fixedDeltaTime);

                if (_CCTimes[i] <= 0.0f)
                    ExitCCFunction?.Invoke((eCrowdControl)i, this);
            }
        }
        
        for (int i = 0; i < 3 && i < _abilNameList.Count; ++i)
        {
            UnitAbilityIconManager.Update(_stateSpriteUIs[i], _abilNameList[i]);
                
            //if (_stateSprites.ContainsKey(_curStateNum[i]) && _stateSpriteUIs[i] != null)
            //{
            //    _stateSpriteUIs[i].sprite = _stateSprites[_curStateNum[i]];
            //}
        }
    }
}

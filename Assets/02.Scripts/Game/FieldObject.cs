using System.Collections;
using UnityEngine;

public enum eCrowdControl
{
    FrostyFeet,
    Freezing,
    Burn,
    Addicte,
    Inspire,
    Wither,

    Last
}

public class FieldObject : MonoBehaviour
{
    public float _curHp;
    public float _maxHp;

    // 아군인지 적군인지에 대한 변수
    // [HideInInspector]
    public eTeam _team = eTeam.PLAYER;

    //죽었는지 살았는지에 대한 변수
    [HideInInspector]
    public bool _isDead;

    public virtual void DamageReceive(float damage, FieldObject receiveObject) { }
   
    public float DefaultCurHealth   = 0;            public virtual float CurHealth { get { return _curHp; } set { } }
    public float DefaultMaxHealth   = 0;            public virtual float MaxHealth { get { return _maxHp; } set { } }
    public float DefaultAttackSpeed = 0;            public virtual float AttackSpeed { get { return 0; } set { } }
    public float DefaultMoveSpeed   = 0;            public virtual float MoveSpeed { get { return 0; } set { } }
    public float DefaultDefensiveCleavage = 0;      public virtual float DefensiveCleavage { get { return 0.0f; } set { } }
    public float RemainHealth => CurHealth / MaxHealth;

    public bool IsDead  
    {
        get => _isDead;
        set {  }
    }

    public bool[] _isCrowdControls = new bool[(int)eCrowdControl.Last];
    public float[] _CCTimes = new float[(int)eCrowdControl.Last];

    public delegate void ExitCC(eCrowdControl type, FieldObject obj);
    public ExitCC ExitCCFunction;

    public void CrowdControl(eCrowdControl curCC, float time)
    {
        _isCrowdControls[(int)curCC] = true;
        _CCTimes[(int)curCC] = time;
    }

    virtual protected void Awake()
    {
        DefaultCurHealth    = CurHealth;
        DefaultMaxHealth    = MaxHealth;
        DefaultAttackSpeed  = AttackSpeed;
        DefaultMoveSpeed    = MoveSpeed;
    }

    virtual protected void FixedUpdate()
    {
        for (int i = 0; i < (int)eCrowdControl.Last; ++i)
        {
            if (_isCrowdControls[i])
            {
                _CCTimes[i] -= Time.fixedDeltaTime;

                if (_CCTimes[i] <= 0.0f)
                {
                    ExitCCFunction((eCrowdControl)i, this);
                }
            }
        }
    }
}

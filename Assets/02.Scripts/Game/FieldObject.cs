using UnityEngine;

public class FieldObject : MonoBehaviour
{
    public float _curHp;
    public float _maxHp;

    //아군인지 적군인지에 대한 변수
    [HideInInspector]
    public eTeam _team = eTeam.PLAYER;

    //죽었는지 살았는지에 대한 변수
    [HideInInspector]
    public bool _isDead;

    public virtual void DamageReceive(float damage) { }
    public float GetCurHealth() { return _curHp; }
    public float GetMaxHealth() { return _maxHp; }

    public bool IsDead
    {
        get => _isDead;
        set {  }
    }
}

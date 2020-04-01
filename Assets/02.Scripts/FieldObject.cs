using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject : MonoBehaviour
{
    public float _curhealth;    public float HP { get { return _curhealth; } }
    public float _maxhealth;    public float maxHP { get { return _maxhealth; } }

    public eTeam _team = eTeam.PLAYER;

    public bool _isdead = false;

    public virtual void DamageReceive(float damage)
    {
    }

    public bool IsDead { get { return _isdead; } set { } }
}

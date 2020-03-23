using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : FieldObject
{
    private void Awake()
    {
        _curhealth = _maxhealth = 100;   
    }

    public override void DamageReceive(float damage)
    {
        _curhealth -= damage;
        

    }

}

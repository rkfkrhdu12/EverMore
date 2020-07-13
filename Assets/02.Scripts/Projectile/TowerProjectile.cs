using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : Projectile
{
    Tower ParentTower;

    protected override void Awake()
    {
        base.Awake();

        ParentTower = Parent.GetComponent<Tower>();
    }

    protected override void FixedUpdate()
    {
        if (Target == null || !Target.gameObject.activeSelf || Target.CurHealth <= 0)
        {
            DeleteObjectSystem.AddDeleteObject(gameObject);
        }

        base.FixedUpdate();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (other.isTrigger) { return; }

        FieldObject obj = other.GetComponent<FieldObject>();
        if (obj == null || Target != obj) { return; }

        if (obj.CurHealth <= 0)
            ParentTower.UpdateTarget();

        DeleteObjectSystem.AddDeleteObject(gameObject);
    }
}

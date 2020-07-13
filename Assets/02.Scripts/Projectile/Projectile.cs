using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : FieldObject
{
    public FieldObject Parent = null;
    public FieldObject Target = null;
    public float AttackDamage = 0.0f;
    float _moveSpeed = 0;
    public override float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    readonly string _layerName = "Unit";

    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer(_layerName);
        gameObject.tag = _layerName;
    }

    protected virtual void Update()
    {
        if (Target == null) { return; }

        transform.position = Vector3.Lerp(transform.position, Target.transform.position, Time.deltaTime * MoveSpeed);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) { return; }

        FieldObject obj = other.GetComponent<FieldObject>();
        if (obj == null || Target != obj) { return; }

        obj.DamageReceive(AttackDamage, Parent);
    }
}

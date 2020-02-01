using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int _curhealth;
    public int _maxhealth;

    public int _attackDamage;
    public float _attackSpeed;

    public float _moveSpeed;

    public bool _isSpawn = false;

    private void FixedUpdate()
    {
        if (!_isSpawn) return;

        transform.Translate(0, 0, _moveSpeed * Time.deltaTime);
    }
}

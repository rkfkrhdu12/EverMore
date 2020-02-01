using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    // Test
    public CreateUnit _playerCreateUnit;
    public CreateUnit _enemyCreateUnit;

    public GameObject _unitObject;

    private void Start()
    {
        Unit unit1 = _unitObject.GetComponent<Unit>();
        unit1._curhealth = 100;
        unit1._maxhealth = 100;
        unit1._moveSpeed = 1;
        unit1._attackDamage = 10;
        unit1._attackSpeed = 1;

        Unit[] units = new Unit[1];
        units[0] = unit1;

        PlayerSetUnit(units);
    }
    
    void PlayerSetUnit(Unit[] units)
    {
        _playerCreateUnit.SetUnit(units);
    }
    
    void EnemySetUnit(Unit[] units)
    {
        _enemyCreateUnit.SetUnit(units);
    }
}

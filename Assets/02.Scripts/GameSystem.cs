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
        Unit a = new Unit();

        Debug.Log("Comp");
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

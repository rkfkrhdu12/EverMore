using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윤용우
public class GameSystem : MonoBehaviour
{
    // 유닛 생성
    [SerializeField]
    CreateUnit _playerCreateUnit;

    [SerializeField]
    CreateUnit _enemyCreateUnit;
    // #

    [SerializeField]
    GameObject _unitObject; //유니티 짱

    private void Start()
    {
        Unit a = new Unit();
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

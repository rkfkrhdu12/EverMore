using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreateUnit : CreateUnit
{
    public GameObject _unitSlot;

    // Player UnitSlot Image 파일을 Inspector 를 통해 직접 할당
    public Image[] imageSlot; 

    void Update()
    {
        if(!_isSpawn)
        {
            _spawnTime += Time.deltaTime;
            if(_spawnInterval <= _spawnTime)
            {
                _spawnTime = 0.0f;
                _isSpawn = true;
            }
        }

        if (Input.GetKey(KeyCode.A))
            GameStart();

        if (Input.GetKey(KeyCode.S) && _isSpawn)
        {
            _isSpawn = false;

            SpawnUnit(0);
        }
    }

    private bool _isStart = false;
    private bool _isSpawn = false;
    private float _spawnTime = .0f;
    private const float _spawnInterval = .5f;
    void GameStart()
    {
        if(_isStart)            { return; }
        else                    { _isStart = true; }
        if (0 >= _curUnitCount) { Debug.LogErrorFormat("Unit이 들어있지 않습니다."); }

        for (int i = _curUnitCount; i < _maxUnitCount; ++i) 
        {
            if (imageSlot.Length - 1 < i) break;

            imageSlot[i].gameObject.SetActive(false);
        }
    }

}

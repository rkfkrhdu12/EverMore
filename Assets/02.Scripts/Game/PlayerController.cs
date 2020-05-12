using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variable
    private float _spawnRange = 7.0f;

    [SerializeField]
    private SpawnManager _spawnManager;

    //public GameObject _prefabs;

    eMouseState _curMouseState;
    private enum eMouseState
    {
        None,
        Camera,
        Spawn,
        Spawning,
    }
    #endregion

    #region Monobehaviour Function

    void Update()
    {
        UpdateMouseState();
    }

    #endregion

    #region Private Function

    void UpdateMouseState()
    {
        switch (_curMouseState)
        {
            case eMouseState.None:                      break;
            case eMouseState.Camera:                    break;
            case eMouseState.Spawn:                     break;
            case eMouseState.Spawning: UpdateSpawnPoint();   break;
        }
    }

    void UpdateSpawn()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            _spawnManager.OnSpawn(0);
        }
    }

    void UpdateSpawnPoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                Vector3 dist = hitInfo.point - transform.position;
                if (dist.sqrMagnitude < _spawnRange * _spawnRange && dist.sqrMagnitude > _spawnRange + _spawnRange)
                { // Spawn
                    // Test
                    //Debug.Log("Access  ");

                    //Destroy(Instantiate(_prefabs, hitInfo.point, Quaternion.identity, null),10);

                }
            }
        }
    }
    #endregion
}

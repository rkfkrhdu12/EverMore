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

    private void Awake()
    {
        _curMouseState = eMouseState.Spawn;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && _curMouseState != eMouseState.Spawning)  { _curMouseState = eMouseState.Camera; }
        else if (_curMouseState == eMouseState.Camera) { _curMouseState = eMouseState.Spawn; }

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
            case eMouseState.Spawn:    UpdateSpawn(); break;
            case eMouseState.Spawning: UpdateSpawnPoint();   break;
        }
    }

    void UpdateSpawn()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _curMouseState = eMouseState.Spawning;

            _spawnManager.SetSpawnIndex(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _curMouseState = eMouseState.Spawning;

            _spawnManager.SetSpawnIndex(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _curMouseState = eMouseState.Spawning;

            _spawnManager.SetSpawnIndex(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _curMouseState = eMouseState.Spawning;

            _spawnManager.SetSpawnIndex(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _curMouseState = eMouseState.Spawning;

            _spawnManager.SetSpawnIndex(4);
        }
    }

    void UpdateSpawnPoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 200))
            {
                Vector3 dist = hitInfo.point - transform.position;

                Debug.Log("" + dist.sqrMagnitude + " < " + (_spawnRange * _spawnRange) + "  "+ dist.sqrMagnitude + " > " + (_spawnRange + _spawnRange));

                if (dist.sqrMagnitude < _spawnRange * _spawnRange && dist.sqrMagnitude > _spawnRange + _spawnRange)
                { // Spawn
                    _spawnManager.SetSpawnPoint(hitInfo.point);

                    _spawnManager.Spawn();

                    Debug.Log("Spawn");
                    _curMouseState = eMouseState.Spawn;
                }
            }
        }
    }
    #endregion
}

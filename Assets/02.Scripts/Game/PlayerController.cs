using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variable
    private float _spawnRange = 7.0f;

    [SerializeField]
    private SpawnManager _spawnManager = null;

    KeyCode[] _spawnKeyCode = new KeyCode[6];

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

        int i = 0;
        _spawnKeyCode[i++] = KeyCode.Alpha1;
        _spawnKeyCode[i++] = KeyCode.Alpha2;
        _spawnKeyCode[i++] = KeyCode.Alpha3;
        _spawnKeyCode[i++] = KeyCode.Alpha4;
        _spawnKeyCode[i++] = KeyCode.Alpha5;
        _spawnKeyCode[i++] = KeyCode.Alpha6;
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
            case eMouseState.None:                                                  break;
            case eMouseState.Camera:                                                break;
            case eMouseState.Spawn:    UpdateSpawnInput();                          break;
            case eMouseState.Spawning: UpdateSpawnInput(); UpdateSpawnPoint();      break;
        }
    }

    public void Spawn(int index)
    {
        _curMouseState = eMouseState.Spawning;

        _spawnManager.SetSpawnIndex(index);
    }

    void UpdateSpawnInput()
    {
        for (int i = 0; i < _spawnKeyCode.Length; ++i) 
        {
            if (Input.GetKeyDown(_spawnKeyCode[i]))
                Spawn(i);
        }
    }

    public int rayDistance = 200;

    void UpdateSpawnPoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Camera mainCamera = Camera.main;

            rayDistance = (int)Mathf.Sqrt(mainCamera.transform.position.sqrMagnitude) + 30;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            int mask = 1 << 11;
            mask = ~mask;

            int deleteMask = 1 << 10;
            deleteMask = ~deleteMask;

            mask -= deleteMask;

            if (Physics.Raycast(ray, out hitInfo, rayDistance, mask)) 
            {
                Vector3 dist = hitInfo.point - transform.position;

                // if (dist.sqrMagnitude < _spawnRange * _spawnRange)
                { // Spawn
                    _spawnManager.SetSpawnPoint(hitInfo.point);

                    if (_spawnManager.Spawn())
                    {
                        _curMouseState = eMouseState.Spawn;
                    }
                }
            }
        }
        else if(Input.GetMouseButtonDown(0) && _curMouseState != eMouseState.Spawning)
        {
            _curMouseState = eMouseState.Camera;
        }
    }
    #endregion
}

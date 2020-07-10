using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool CostConsumption(int consumCost) { return _costMgr.CostConsumption(consumCost); }
    public void OnLevelUp() { _levelMgr.OnLevelUp(); }

    public LevelManager _levelMgr;
    public CostManager _costMgr;

    public SpawnManager _pSpawnMgr;

    private void Awake()
    {
        _levelMgr.Init(_costMgr);
        _costMgr.Init(_levelMgr);
    }
}

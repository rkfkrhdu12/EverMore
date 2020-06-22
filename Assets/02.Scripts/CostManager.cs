using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostManager : MonoBehaviour
{
    public bool CostConsumption(int consumCost)
    {
        if (consumCost > CurCost)
        {
            return false;
        }

        CurCost -= consumCost;

        return true;
    }

    public void Init(SpawnManager spawnMgr, in LevelManager.LevelData levelData)
    {
        _spawnMgr = spawnMgr;
        _levelData = levelData;
    }

    public void Enable()
    {
        for (int i = 0; i < _spawnMgr._teamUnits.Length; ++i)
        {
            int headNum = _spawnMgr._teamUnits.GetUnit(i)._equipedItems[0];

            UnitIconManager.Update(_iconObjects[i], headNum);

            UnitStatus uStatus = _spawnMgr._teamUnits.GetUnit(i);

            uStatus.UpdateItems();

            _unitCostTexts[i].text = uStatus._cost.ToString();
        }
    }

    #region Variable

    [SerializeField]
    private Image _costImage = null;

    [SerializeField]
    private TMP_Text _costText;

    public GameObject[] _iconObjects;
    public TMP_Text[] _unitCostTexts;

    private SpawnManager _spawnMgr;

    private LevelManager.LevelData _levelData;

    private float _curCost;
    private float CurCost
    {
        get { return _curCost; }
        set
        {
            _curCost = value;

            int maxCost = _levelData._maxCost;

            _costImage.fillAmount = _curCost / maxCost;
            _costText.text = ((int)_curCost).ToString() + " / " + maxCost.ToString();
        }
    }
    #endregion

    private void Update()
    {
        CurCost = Mathf.Clamp(CurCost + Time.deltaTime * _levelData._costRegen, 0, _levelData._maxCost);
    }
}

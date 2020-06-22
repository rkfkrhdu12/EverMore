using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

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

    public void Init(LevelManager levelData)
    {
        _levelMgr = levelData;
    }
    public void Enable(SpawnManager spawnMgr)
    {
        _curCost = 100;

        for (int i = 0; i < spawnMgr._teamUnits.Length; ++i)
        {
            int headNum = spawnMgr._teamUnits.GetUnit(i)._equipedItems[0];

            UnitIconManager.Update(_iconObjects[i], headNum);

            UnitStatus uStatus = spawnMgr._teamUnits.GetUnit(i);

            _unitCostTexts[i].text = uStatus._cost.ToString();
        }
    }

    public void UpdateIcon(int index, bool isCoolDown)
    {
        if(isCoolDown)
        { // 쿨타임 중
            UnitIconManager.SetColor(_iconObjects[index], Color.gray);
        }
        else
        { // 이 아님
            UnitIconManager.SetColor(_iconObjects[index], Color.white);
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

    private LevelManager _levelMgr;

    private float _curCost;
    public float CurCost
    {
        get { return _curCost; }
        set
        {
            _curCost = value;

            int maxCost = _levelMgr._maxCost;

            _costImage.fillAmount = _curCost / maxCost;

            StringBuilder sb = new StringBuilder();
            sb.Append((int)_curCost).Append(" / ").Append(maxCost);
            _costText.text = sb.ToString();
        }
    }
    #endregion

    private void Update()
    {
        CurCost = Mathf.Min(CurCost + Time.deltaTime * _levelMgr._costRegen, _levelMgr._maxCost);
    }
}

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

    [SerializeField]
    float _startCost = 100;

    public void Enable(SpawnManager spawnMgr)
    {
        _curCost = _startCost;

        for (int i = 0; i < spawnMgr._teamUnits.Length; ++i)
        {
            int headNum = spawnMgr._teamUnits.GetUnit(i)._equipedItems[0];

            UnitIconManager.Update(_iconObjects[i], headNum);

            UnitStatus uStatus = spawnMgr._teamUnits.GetUnit(i);

            _unitCostTexts[i].text = uStatus._cost.ToString();
        }
    }

    #region Variable
    private float[] _coolDownTime = new float[6];
    public void SetCoolDownUI(int index, float coolDown)
    {
        if (_iconObjects.Length <= index || _iconCoolTimeImage.Length <= index) { LogMessage.LogError("UpdateIcon index is Over"); return; }

        _coolDownTime[index] = coolDown;
    }

    [SerializeField]
    private Image _costImage = null;

    [SerializeField]
    private TMP_Text _costText;

    public GameObject[] _iconObjects;
    public Image[] _iconCoolTimeImage;
    
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

        for (int i = 0; i < _coolDownTime.Length; ++i)
        {
            if(_coolDownTime[i] >= 0)
            {
                _coolDownTime[i] -= Time.deltaTime;

                _iconCoolTimeImage[i].fillAmount = 0.2f + _coolDownTime[i] * .6f;
            }
        }
    }
}

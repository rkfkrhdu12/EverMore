using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System.Linq;

public class LevelManager : MonoBehaviour
{
    public void OnLevelUp()
    {
        if (_curLevel >= _levelDataList.Count) { return; }
        if (!_costMgr.CostConsumption(_curLevelData._upgradeCost)) { return; } //  && _curLevel != 0

        if (_curLevel < 0) { _curLevel = 0; }

        ++_curLevel;

        if (_curLevel == _levelDataList.Count)
        {
            _levelBG.color          = Color.gray;
            _levelText.color        = Color.gray;
            _levelUpCostText.color  = Color.gray;

            _levelText.text = "Max";
            _levelUpCostText.text = " -";
        }
        else
        {
            _levelText.text = " " + _curLevel.ToString();
            _levelUpCostText.text = _upgradeCost.ToString();
        }
    }

    public void OnButtonDown()
    {
        _levelText.transform.localScale = new Vector3(.88f, .88f, .88f);
        _levelUpCostText.transform.localScale = new Vector3(.88f, .88f, .88f);
    }

    public void OnButtonUp()
    {
        _levelText.transform.localScale = Vector3.one;
        _levelUpCostText.transform.localScale = Vector3.one;
    }

    public void Init(CostManager costMgr)
    {
        _costMgr = costMgr;
        InitLevelData();
    }

    public void Enable()
    {
        _levelText.text = " " + _curLevel.ToString();
        _levelUpCostText.text = _upgradeCost.ToString();
    }

    #region Variable

    public Image _levelBG;

    [SerializeField]
    private TMP_Text _levelUpCostText;

    [SerializeField]
    private TMP_Text _levelText;

    CostManager _costMgr;

    private int _curLevel = 1;

    public int _maxCost { get { return _curLevelData._maxCost; } }
    public float _costRegen { get { return _curLevelData._costRegen; } }
    public int _upgradeCost { get { return _curLevelData._upgradeCost; } }

    // _curLevel 이 현재 존재하지 않는 레벨이거나 아직 Init이 안되었다면 new LevelData() return
    public LevelData _curLevelData => _levelDataList.ContainsKey(_curLevel) ? _levelDataList[_curLevel] : (_levelDataList.ContainsKey(1) ? _levelDataList[1] : new LevelData());

    // 굳이 Dictionary 일 필요가 있을까 ? 
    Dictionary<int, LevelData> _levelDataList = new Dictionary<int, LevelData>();

    public struct LevelData
    {
        public int _maxCost;
        public float _costRegen;
        public int _upgradeCost;
    }
    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        InitLevelData();
    } 
    #endregion

    private void InitLevelData()
    {
        if (_levelDataList.Count != 0) { return; }

        List<string> levelDatas = CSVParser.Read("CostTable");
        if (null == levelDatas) { LogMassage.Log("CostTable.csv is Error"); return; }

        // using System.Linq;
        foreach (var splitDatas in levelDatas.Select(t => t.Split(',')))
        {
            LevelData newData;

            int.TryParse(splitDatas[1], out newData._maxCost);
            float.TryParse(splitDatas[2], out newData._costRegen);
            int.TryParse(splitDatas[3], out newData._upgradeCost);

            _levelDataList.Add(int.Parse(splitDatas[0]), newData);
        }
    }
}

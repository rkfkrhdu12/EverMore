using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public void SetAddTeamName(string teamName)
    {
        _addTeamName = teamName;
    }

    #region Private Variable

    [SerializeField]
    private MainSceneUI.MainSceneManager _sceneMgr = null;

    [SerializeField]
    private TeamSelectionSystem _teamSelectSystem = null;

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();


    [Header((MainSceneUI.UIDataKey.ChoiceUnit) + " UI")]
    // ChoiceUnit
    [SerializeField,Tooltip("현재 선택된 팀 이름")]
    private string _curSelectTeamName;

    [SerializeField,Tooltip("유닛슬롯")]
    private RawImage[] _slotImage;

    [SerializeField]
    private Sprite _UnitAddImage;

    [SerializeField, Tooltip("팀이름 UI")]
    private TMP_Text _choiceUnitTeamNameUI = null;

    [SerializeField]
    private TeamSelectionSystem _teamSelectionSystem;

    [Header((MainSceneUI.UIDataKey.AddTeam) + " UI")]
    // AddTeam
    [SerializeField]
    private TMP_InputField _teamAddInputField;

    [SerializeField,Tooltip("추가될 팀 이름")]
    private string _addTeamName;

    [Header(MainSceneUI.UIDataKey.SetUnit + " UI")]
    // SetUnit
    int _curUnitNum = 0;

    [SerializeField]
    private GameObject _SetUnitObject = null;

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        string teamName = "예비1팀";
        Team t = new Team();
        t.Name = teamName;

        _teams.Add(teamName, t);
        _teamNameList.Add(teamName);
    }

    #endregion

    #region OnClickEvent Function

    
    // Choice Team
    public void OnSelectTeamName(TMP_Text text)     
    {
        string teamName = text.text;

        if (string.IsNullOrEmpty(teamName)) { return; }
        if (!_teams.ContainsKey(teamName)) { return; }

        _curSelectTeamName = teamName;
    }
    
    // Add Team
    public void OnAddTeam()                         
    {
        if(_teams.ContainsKey(_addTeamName)) { return; }

        string teamName = _addTeamName;

        Team t = new Team();
        t.Name = teamName;

        _teams.Add(teamName, t);
        _teamNameList.Add(teamName);

        _teamSelectionSystem.AddButton(_teamAddInputField);
    }

    // Choice Unit
    public void OnDeleteTeam()                      
    {
        // Team 데이터는 가비지 컬렉터가 지워줄꺼라 믿숩니다 ............
        _teams.Remove(_curSelectTeamName);
    }
    public void OnSetTeamNameUI()                   
    {
        _choiceUnitTeamNameUI.text = _teams[_curSelectTeamName].Name;
    }
    public void OnUpdateChoiceUnitUI()              
    {
        int unitCount = _teams[_curSelectTeamName].Length;

        if (unitCount > _slotImage.Length) { Debug.Log("TeamManager : OnUpdateUI UnitCount Error"); return; }

        for (int i = 0; i < _slotImage.Length; ++i)
        {
            //Texture t = null == _teams[_curSelectTeamName].GetUnitTexture(i) ? 
            //                    null : _teams[_curSelectTeamName].GetUnitTexture(i);

            _slotImage[i].texture = /*null != t ? t : */_UnitAddImage.texture;
            _slotImage[i].SetNativeSize();
        }
    }

    // SetUnit
    public void OnUpdateUnitModel(int index)        
    {
        if(index >= _teams[_curSelectTeamName].Length|| index < 0) { return; }

        _curUnitNum = index;

        UnitController curSelectUnit = _teams[_curSelectTeamName].GetUnit(_curUnitNum);

        UnitModelManager UMMgr = new UnitModelManager();
        UMMgr.UpdateModel(_SetUnitObject, curSelectUnit._status._equiedItems);
    }

    #endregion
}
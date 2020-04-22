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

    [Header((MainSceneUI.UIDataKey.ChoiceUnit) + " UI")]

    [SerializeField,Tooltip("현재 선택된 팀 이름")]
    private string _curSelectTeamName;

    [SerializeField,Tooltip("유닛슬롯")]
    private RawImage[] _slotImage;

    [SerializeField]
    private Sprite _UnitAddImage;

    [SerializeField, Tooltip("팀이름 UI")]
    private TMP_Text _choiceUnitTeamNameUI = null;

    [Header((MainSceneUI.UIDataKey.AddTeam) + " UI")]

    [SerializeField,Tooltip("추가될 팀 이름")]
    private string _addTeamName;

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();
    #endregion

    #region OnClickEvent Function

    public void OnUpdateUI()
    {
        int unitCount = _teams[_curSelectTeamName]._units.Count;
    }
    public void OnSelectTeamName(TMP_Text text)
    {
        string teamName = text.text;

        if (string.IsNullOrEmpty(teamName)) { return; }
        if (!_teams.ContainsKey(teamName)) { return; }

        _curSelectTeamName = teamName;
    }
    public void OnDeleteTeam()
    {
        // Team 데이터는 가비지 컬렉터가 지워줄꺼라 믿숩니다 ............
        _teams.Remove(_curSelectTeamName);
    }
    public void OnAddTeam()
    {
        string teamName = _addTeamName;

        Team t = new Team();
        t.Name = teamName;

        _teams.Add(teamName, t);
        _teamNameList.Add(teamName);
    }
    public void OnSetTeamNameUI()
    {
        _choiceUnitTeamNameUI.text = _teams[_curSelectTeamName].Name;
    } 

    #endregion
}
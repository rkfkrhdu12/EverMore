using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamManager : MonoBehaviour
{
    public Team GetTeam(string teamName)
    {
        if (_teams.ContainsKey(teamName)) { return null; }

        return _teams[teamName];
    }

    public Team _curSelectTeam;

    [SerializeField]
    private string _addTeamName;

    public void SetAddTeamName(string teamName)
    {
        _addTeamName = teamName;
    }

    [SerializeField]
    private TeamSelectionSystem _teamSelectSystem = null;
    
    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();

    [SerializeField]
    private MainScene.MainSceneManager _sceneMgr = null;

    public void SelectTeamName(TMP_Text text)
    {

    }

    private void UpdateUI()
    {

    }

    public void DeleteTeam()
    {

    }

    public void AddTeam()
    {
        string teamName = _addTeamName;

        AddTeam(teamName);
    }

    public void AddTeam(string teamName)
    {
        if(string.IsNullOrEmpty(teamName)) { return; }

        Team t = new Team();
        t.Name = teamName;

        _teams.Add(teamName, t);
        _teamNameList.Add(teamName);
    }
}

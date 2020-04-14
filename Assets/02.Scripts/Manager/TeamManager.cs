using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamManager : MonoBehaviour
{
    private string _addTeamNameString = "예비1팀";

    public void SetAddTeamNameString(string name) { _addTeamNameString = name; }
    public string GetAddTeamNameString()
    {
        string returnVal = _addTeamNameString;

        _addTeamNameString = "";
        _textObject.GetComponent<TextMeshPro>().text = "";

        return returnVal;
    }

    public Team _curSelectTeam = null;

    public GameObject _textObject;

    public OperateButtonGroup _buttonGroup;

    private void Awake()
    {
        AddTeam(GetAddTeamNameString());
    }

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();

    public Team GetTeam(string teamName)
    {
        if (_teams.ContainsKey(teamName)) { return null; }

        return _teams[teamName];
    }

    public void SelectTeam(GameObject gObject)
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
        string teamName = GetAddTeamNameString();
        AddTeam(teamName);
    }

    public void AddTeam(string teamName)
    {
        if(string.IsNullOrEmpty(teamName)) { return; }

        Team t = new Team();
        t.Name = teamName;

        _teams.Add(teamName, t);
        _teamNameList.Add(teamName);

        // Test
        _teams[teamName].InitTest();
    }
}

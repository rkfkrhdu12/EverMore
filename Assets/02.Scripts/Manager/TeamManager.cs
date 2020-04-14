using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamManager : MonoBehaviour
{
    private string _addTeamNameString = "";

    public void SetAddTeamNameString(string name) { _addTeamNameString = name; }
    public string GetAddTeamNameString()          { _textObject.GetComponent<TextMeshPro>().text = ""; return _addTeamNameString; }

    public GameObject _textObject;

    public OperateButtonGroup _buttonGroup;

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();

    public Team GetTeam(string teamName)
    {
        if (_teams.ContainsKey(teamName)) { return null; }

        return _teams[teamName];
    }


}

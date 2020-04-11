using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    [SerializeField,Tooltip("팀 버튼을 눌렀을때 생길 오브젝트")]
    private GameObject _teamPrefab = null;

    [SerializeField, Tooltip("팀 선택 뷰포트")]
    private GameObject _viewPort = null;

    public void AddTeam(string name)
    {
        GameObject clone = Instantiate(_teamPrefab, _viewPort.transform);
        clone.GetComponent<Text>().text = name;
        // 팀만들기 시스템 구현중 ..
        // 버튼 이미지 전부 세팅(Team (n))
        //  string(name) 받는 시스템 구현(엔진)
    }

    #region Private Variable
    private List<Team> _teams;

    private int _teamCount = 0;

    private readonly string _prefKey = "TeamNumber";


    #endregion

    private void Awake()
    {
        for (string i = PlayerPrefs.GetString(_prefKey + _teamCount.ToString());
            !string.IsNullOrEmpty(i);
            ++_teamCount, i = PlayerPrefs.GetString(_prefKey + _teamCount.ToString()))
        {

        }
    }


    private void Encryption(Team teamData)
    {

    }

}

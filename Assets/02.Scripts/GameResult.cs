using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject _victoryObj;
    public GameObject _defeatObj;

    private void Awake()
    {
        switch(GameManager.Instance._WinTeam)
        {
            case eTeam.PLAYER: _victoryObj.SetActive(true);     break;
            case eTeam.ENEMY:  _defeatObj.SetActive(true);      break;
        }
    }

    public void NextScene()
    {
        GameManager.Instance.NextScene();
    }
}

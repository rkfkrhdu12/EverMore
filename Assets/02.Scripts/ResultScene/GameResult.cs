using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject _victoryObj;
    public GameObject _defeatObj;

    private void Awake()
    {
       //switch(Manager.Get<Manager>()._WinTeam)
        // {
        //     case eTeam.PLAYER: _victoryObj.SetActive(true);     break;
        //     case eTeam.ENEMY:  _defeatObj.SetActive(true);      break;
        // }
    }

    public void NextScene()
    {
      //  Manager.Get<GameManager>().NextGoto();
    }
}

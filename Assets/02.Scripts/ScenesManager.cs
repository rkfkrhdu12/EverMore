using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    static eScene curScene = eScene.MAIN;
    eScene _curScene { get { return curScene; } set { curScene = value; LoadScene(curScene); } }
    public enum eScene
    {
        MAIN,
        OPTION,
        CUSTOM,
        GAMESEARCH,
        INGAME,
    }
    
    public void NextScene(string nextSceneName = "")
    {
        switch (_curScene)
        {
            case eScene.MAIN:
            {
                switch (nextSceneName)
                {
                    //case _optionSceneName: _curScene = eScene.OPTION; break;
                    case _customSceneName: _curScene = eScene.CUSTOM; break;
                    //case _gameSearchSceneName: _curScene = eScene.GAMESEARCH; break;
                }
                break;
            }
            case eScene.GAMESEARCH: _curScene = eScene.INGAME; break;
            //case eScene.INGAME:     LoadScene("");    break;
        }
    }

    public void PrevScene()
    {
        switch (_curScene)
        {
            case eScene.OPTION:     _curScene = eScene.MAIN;        break;
            case eScene.CUSTOM:     _curScene = eScene.MAIN;        break;
            case eScene.GAMESEARCH: _curScene = eScene.MAIN;    break;
        }
    }

    const string _mainSceneName         = "MainScene";
    const string _optionSceneName       = "OptionScene";
    const string _customSceneName       = "CustomScene";
    const string _gameSearchSceneName   = "GameSearchScene";
    const string _ingameSceneName       = "IngameScene";
    const string _gameResultScene       = "GameResultScene";

    private void LoadScene(eScene scene)
    {
        string sceneName = "";
        // 예외처리를 확실하게 하기 위해
        switch (scene)
        {
            case eScene.MAIN:         sceneName = _mainSceneName;           break;
            case eScene.OPTION:       sceneName = _optionSceneName;         break;
            case eScene.CUSTOM:       sceneName = _customSceneName;         break;
            case eScene.GAMESEARCH:   sceneName = _gameSearchSceneName;     break;
            case eScene.INGAME:       sceneName = _ingameSceneName;         break;
            default: return;
        }

        SceneManager.LoadScene(sceneName);
    }
}

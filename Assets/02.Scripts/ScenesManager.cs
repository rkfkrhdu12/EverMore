using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager
{
    public void Start()
    {
        _buttonCantAlphaHit.Start();
    }

    public void NextScene(string nextSceneName = "")
    {
        switch (_curScene)
        {
            case eScene.LOBBY:
            {
                switch (nextSceneName)
                {
                    //case _optionSceneName: _curScene = eScene.OPTION; break;
                    case _gameSearchSceneName: _curScene = eScene.INGAME; break;
                }
                break;
            }
            case eScene.GAMESEARCH: _curScene = eScene.INGAME; break;
            case eScene.INGAME:     _curScene = eScene.GAMERESULT;    break;
            case eScene.GAMERESULT: _curScene = eScene.LOBBY; break;
        }
    }

    //public void NextScene(Image image)
    //{
    //    _ScreensManager.UpdateButton(image);
    //}

    public void PrevScene()
    {
        switch (_curScene)
        {
            //case eScene.OPTION:     _curScene = eScene.MAIN;        break;
            //case eScene.CUSTOM:     _curScene = eScene.MAIN;        break;
            //case eScene.GAMESEARCH: _curScene = eScene.MAIN;        break;
        }
    }

    public eScene GetCurScene()
    {
        return _curScene;
    }

    #region Variable

    eScene curScene = eScene.LOBBY;
    eScene _curScene { get { return curScene; } set { curScene = value; LoadScene(curScene); } }
    public enum eScene
    {
        LOBBY,
        OPTION,
        GAMESEARCH,
        INGAME,
        GAMERESULT,
    }

    ButtonCantAlphaHit _buttonCantAlphaHit = new ButtonCantAlphaHit();

    //ScreensManager _screensManager = new ScreensManager();

    const string _mainSceneName = "MainScene";
    //const string _optionSceneName = "OptionScene";
    //const string _customSceneName = "CustomScene";
    const string _gameSearchSceneName = "GameSearchScene";
    const string _ingameSceneName = "IngameScene";
    const string _gameResultScene = "GameResultScene";
    
    #endregion

    #region Private Fuction

    private void LoadScene(eScene scene)
    {
        string sceneName = "";

        //_screensManager.ResetSprite();

        // 예외처리를 확실하게 하기 위해
        switch (scene)
        {
            case eScene.LOBBY: sceneName = _mainSceneName; break;
            //case eScene.OPTION: sceneName = _optionSceneName; break;
            //case eScene.CUSTOM: sceneName = _customSceneName; break;
            case eScene.GAMESEARCH: sceneName = _gameSearchSceneName; break;
            case eScene.INGAME: sceneName = _ingameSceneName; break;
            case eScene.GAMERESULT: sceneName = _gameResultScene; break;
            default: return;
        }

        SceneManager.LoadScene(sceneName);

        _buttonCantAlphaHit.Start();
    } 
    #endregion
}

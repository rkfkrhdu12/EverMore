using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class eSceneString : SerializableDictionary<SceneManagerPro.eScene, string>
{
}

[ManagerDefaultPrefab("SceneManagerPro")]
public class SceneManagerPro : Manager
{
    #region Enum

    public enum eScene
    {
        LOBBY,
        OPTION,
        GAMESEARCH,
        INGAME,
        GAMERESULT,
        MainScene,
    }

    #endregion

    #region Show Inspector

    [Tooltip("장소와 씬 이름을 매치")]
    public eSceneString SceneName;

    #endregion

    #region Hide Inspector

    private eScene curScene = eScene.LOBBY;

    #endregion

    #region NextScene

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //딕셔너리에 해당 씬 이름이 정의 되있는지 체크, 있으면 true, 없으면 false
        if (SceneName.ContainsValue(sceneName))
            LogMessage.Log("Y bug ?");
        else
            LogMessage.LogWarning("This is an undefined scene name.");
    }

    public void LoadScene(eScene scene) =>
        SceneManager.LoadScene(SceneName[scene]);

    #endregion

    public void PrevScene() => //이전 씬으로 이동
        SceneManager.LoadScene(Mathf.Clamp(SceneManager.GetActiveScene().buildIndex - 1, 0, int.MaxValue));

    //현재 씬을 반환 합니다.
    public eScene GetCurScene() => curScene;
}

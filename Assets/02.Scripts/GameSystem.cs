using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class GameSystem : MonoBehaviour
{
    static GameSystem _instance;
    public static GameSystem Instance 
    {
        get
        {
            if(null == _instance)
            {
                GameObject gSystemObj = GameObject.Find("GameSystem");
                if(null == gSystemObj) { return null; }
                gSystemObj.SetActive(true);

                _instance = gSystemObj.GetComponent<GameSystem>();

                _instance.enabled = true;

                _instance.OnAwake();
            }
            return _instance;
        }
    }

    Team _playerTeam;
    public int GetPlayerUnitCount() { return _playerTeam._units.Length; }
    public Unit GetPlayerUnit(int value)
    {
        if(value < 0 || value >= _playerTeam._units.Length) { return null; }

        return _playerTeam._units[value];
    }

    ItemList _itemList;
    public ItemList itemList { get { return _itemList; } }

    DeleteObjectManager _deleteObjectManager;

    ScenesManager _scenesManager;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _instance.OnAwake();
        }
    }

    private void OnAwake()
    {
        DontDestroyOnLoad(gameObject);

        _itemList = new ItemList();
        _itemList.Init();

        _deleteObjectManager = new DeleteObjectManager();

        _scenesManager = new ScenesManager();
        _scenesManager.Start();

        _playerTeam = new Team();
        _playerTeam.InitTest();

        UnitStability.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("a");

            _scenesManager.PrevScene();
        }
    }

    private void LateUpdate()
    {
        // DeleteObjectManager
        {
            if (0 < _deleteObjectManager.GetCount())
            {
                GameObject deleteObj = _deleteObjectManager.Dequeue();

                Destroy(deleteObj);
            }
        }
    }


    #region ScenesManager

    public void NextScene(string sceneName)
    {
        while (0 < _deleteObjectManager.GetCount())
        {
            GameObject deleteObj = _deleteObjectManager.Dequeue();

            Destroy(deleteObj);
        }

        _scenesManager.NextScene(sceneName);
    }

    //public void NextScene(Image image)
    //{
    //    _scenesManager.NextScene(image);
    //}

    public void NextScene()
    {

    }

    public void PrevScene()
    {
        _scenesManager.PrevScene();
    }


    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance 
    {
        get
        {
            if(null == _instance)
            {
                GameObject gSystemObj = GameObject.Find("GameSystem");
                if(null == gSystemObj) { return null; }
                gSystemObj.SetActive(true);

                _instance = gSystemObj.GetComponent<GameManager>();

                _instance.enabled = true;

                _instance.OnAwake();
            }
            return _instance;
        }
    }

    public eTeam _WinTeam = eTeam.PLAYER;

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
        if (null == _instance)
        {
            _instance = this;
            _instance.OnAwake();
        }
        else
        {
            Destroy(gameObject);
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
        while (0 < _deleteObjectManager.GetCount())
        {
            GameObject deleteObj = _deleteObjectManager.Dequeue();

            Destroy(deleteObj);
        }

        _scenesManager.NextScene();
    }

    public void PrevScene()
    {
        _scenesManager.PrevScene();
    }


    #endregion

}

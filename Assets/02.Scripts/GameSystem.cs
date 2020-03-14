using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윤용우
public class GameSystem : MonoBehaviour
{
    static GameSystem _instance;
    public static GameSystem Instance 
    {
        get
        {
            if(null == _instance)
            {
                _instance = GameObject.Find("GameSystem").GetComponent<GameSystem>();

                _instance.OnAwake();
            }
            return _instance;
        }
    }

    // 유닛 생성
    [SerializeField]
    CreateUnit _playerCreateUnit;

    [SerializeField]
    CreateUnit _enemyCreateUnit;
    // #

    [SerializeField]
    GameObject _unitObject; //유니티 짱

    ItemList _itemList;
    public ItemList itemList { get { return _itemList; } }

    DeleteObjectManager _deleteObjectManager;

    ScenesManager _scenesManager;

    private void Awake()
    {
        if(_instance == null)
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

        _scenesManager = GetComponent<ScenesManager>();
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

    #region Test

    void PlayerSetUnit(Unit[] units)
    {
        _playerCreateUnit.SetUnit(units);
    }
    
    void EnemySetUnit(Unit[] units)
    {
        _enemyCreateUnit.SetUnit(units);
    }

    #endregion


    #region SCENESMANAGER

    public void NextScene()
    {
        _scenesManager.NextScene();
    }

    public void PrevScene()
    {
        _scenesManager.PrevScene();
    }


    #endregion

}

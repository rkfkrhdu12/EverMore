using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using GameplayIngredients;
using UnityEngine;

[Serializable]
public class stringTexture2D : SerializableDictionary<string, Texture2D>
{
}


[ManagerDefaultPrefab("GameManager")]
public class GameManager : Manager
{
    public void SetPlayerUnits(Team playerTeam) { _playerTeam = playerTeam; }
    public Team GetPlayerUnits()                { return _playerTeam; }

    public void SetEnemyUnitData(Stage.StageData enemyStageData) { _enemyStageData = enemyStageData; }
    public Stage.StageData GetEnemyUnitData() { return _enemyStageData; }

    #region Show Inspector

    //[Tooltip("아군인지 적군인지 선택")]
    //public eTeam _WinTeam = eTeam.PLAYER;

    [SerializeField]
    private AssetLabelReference unitPhotos;

    public stringTexture2D st;

    #endregion

    #region Hide Inspector

    //아이템에 대한 리스트 변수
    public ItemList itemList { get; private set; }

    [HideInInspector]
    public int money = 123456789; //게임속 돈의 재화이다.

    //팀에 대한 변수
    private Team _playerTeam;
    private Stage.StageData _enemyStageData;

    public bool _stageClear = false;

    //삭제에 대한 매니저 변수
    private DeleteObjectSystem _deleteObjectSystem;

    //배경 음악 볼륨이다.
    [Range(0f, 1f)]
    public float _BgmVolume = 0.7f;

    //이펙트 음악 볼륨이다.
    [Range(0f, 1f)]
    public float _SfxVolume = 0.7f;

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        OnAwake();

        //StartCoroutine(getUnitTexture());
    }

    private void OnAwake()
    {
        Application.targetFrameRate = 60;
        _stageClear = false;

        //itemList : 초기화
        itemList = new ItemList();
        itemList.Init();

        _deleteObjectSystem = new DeleteObjectSystem();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Team : 초기화
        _playerTeam = new Team();
        _playerTeam.Init();
        _playerTeam.UpdateItems();

        _enemyStageData = new Stage.StageData();
        _enemyStageData.Team = new Team();
        _enemyStageData.Team.Init(eTeam.ENEMY);
        _enemyStageData.Team.UpdateItems();
    }

    #endregion

}

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
    #region Show Inspector

    [Tooltip("아군인지 적군인지 선택")]
    public eTeam _WinTeam = eTeam.PLAYER;

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

    //삭제에 대한 매니저 변수
    private DeleteObjectSystem _deleteObjectSystem;

    //배경 음악 볼륨이다.
    [Range(0f, 1f)]
    public float _BgmVolume = 0.7f;

    //이펙트 음악 볼륨이다.
    [Range(0f, 1f)]
    public float _SfxVolume = 0.7f;

    #endregion

    /// <summary>
    /// int(플레이어 유닛 갯수) return
    /// </summary>
    /// <returns></returns>
    public int getPlayerUnitCount() //플레이어의 유닛 개수를 리턴합니다.
        => _playerTeam._units.Length;

    /// <summary>
    /// UnitData(유닛) return
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    //index번째 유닛에 대한 데이터를 리턴합니다.
    public UnitData getPlayerUnit(int index)
    {
        //index 값이 0미만이거나, 배열의 길이를 초과한다면 : null을 반환
        if (index < 0 || index >= _playerTeam._units.Length)
            return null;

        //해당 index번째 유닛을 리턴합니다.
        return _playerTeam._units[index];
    }

    #region Monobehaviour Function

    private void Awake()
    {
        OnAwake();

        StartCoroutine(getUnitTexture());
    }

    private void OnAwake()
    {
        //itemList : 초기화
        itemList = new ItemList();
        itemList.Init();

        //PlayerTeam : 초기화
        _playerTeam = new Team();
        //_playerTeam.InitTest();

        UnitStability.Init();
        _deleteObjectSystem = new DeleteObjectSystem();
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate() =>
        objectToDelete();

    #endregion

    #region Private Function

    private IEnumerator getUnitTexture()
    {
        //유닛 텍스쳐 리소스를 가져옵니다.
        Addressables.LoadResourceLocationsAsync(unitPhotos).Completed += op =>
        {
            foreach (var data in op.Result)
                Addressables.LoadAssetAsync<Texture2D>(data.PrimaryKey).Completed += handle =>
                    st.Add(data.PrimaryKey, handle.Result);
        };

        //1초 정도 대기
        yield return new WaitForSeconds(1f);

        //리소스를 해제합니다.
        foreach (var texture2d in st.Values)
            Addressables.Release(texture2d);
    }

    private void objectToDelete()
    {
        //삭제해야하는 오브젝트 목록이 0이라면, 아래 코드 구문 실행 X
        if (DeleteObjectSystem.getCount() <= 0) return;

        //오브젝트를 제거하고, 제거된 오브젝트를 return합니다.
        var deleteObj = DeleteObjectSystem.getDequeue();

        //삭제
        Destroy(deleteObj);
    } 
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _unitPrefabs = null;

    [SerializeField]
    private eTeam _curSpawnTeam = eTeam.PLAYER;

    [SerializeField]
    private Vector3 _spawnPoint;

    public Team _teamUnits;

    [SerializeField]
    private SpawnManager _enemySpawnManager;

    int _curSpawnIndex = 0;

    public void SetSpawnIndex(int index) => _curSpawnIndex = index;

    public void SetSpawnPoint(Vector3 pos) => _spawnPoint = pos;
    public Vector3 GetCastlePosition() { return transform.position; }

    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void Spawn()
    {
        GameObject clone = Instantiate(_unitPrefabs, _spawnPoint, Quaternion.identity, null);

        clone.SetActive(false);

        UnitController unitCtrl = clone.GetComponent<UnitController>();

        unitCtrl._enemyCastlePosition = _enemySpawnManager.GetCastlePosition();

        unitCtrl._status = _teamUnits.GetUnit(_curSpawnIndex);

        UnitModelManager.UpdateModel(clone.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        clone.SetActive(true);
        unitCtrl.Spawn();
    }

    #region Monobehaviour Function

    private void Awake()
    {
        //// Test
        //_teamUnits = new Team();
        //_teamUnits.Init();

        //int[] items = new int[4];
        //items[0] = 3;
        //items[1] = 4;
        //_teamUnits.SetEquipedItems(0,items);

        //items = new int[4];
        //items[0] = 7;
        //items[1] = 8;
        //_teamUnits.SetEquipedItems(1, items);

        //items = new int[4];
        //items[0] = 27;
        //items[1] = 28;
        //_teamUnits.SetEquipedItems(2, items);

        //items = new int[4];
        //items[0] = 29;
        //items[1] = 30;
        //_teamUnits.SetEquipedItems(3, items);

        _teamUnits = Manager.Get<GameManager>().GetPlayerUnits();
    }

    #endregion

}

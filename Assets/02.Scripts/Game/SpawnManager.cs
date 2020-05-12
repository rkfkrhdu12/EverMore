using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // = null > inspector / code 
    [SerializeField]
    private GameObject _unitPrefabs = null;

    [SerializeField]
    private eTeam _curSpawnTeam = eTeam.PLAYER;

    [SerializeField]
    private Vector3 _spawnPoint;

    public Team _teamUnits;

    [SerializeField]
    private SpawnManager _enemySpawnManager;

    public void SetSpawnPoint(Vector3 pos) => _spawnPoint = pos;
    public Vector3 GetCastlePosition() { return transform.position; }

    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void OnSpawn(int index)
    {
        GameObject clone = Instantiate(_unitPrefabs, _spawnPoint, Quaternion.identity, null);

        clone.SetActive(false);

        UnitController unitCtrl = clone.GetComponent<UnitController>();

        unitCtrl._enemyCastlePosition = _enemySpawnManager.GetCastlePosition();

        unitCtrl._status = _teamUnits.GetUnit(index);

        UnitModelManager.UpdateModel(unitCtrl.gameObject, unitCtrl._status._equipedItems);

        clone.SetActive(true);
        unitCtrl.Spawn();
    }

    #region Private Function

    #endregion
}

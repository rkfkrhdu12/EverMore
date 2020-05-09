using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private eTeam _curSpawnTeam = eTeam.PLAYER;

    [SerializeField]
    private Vector3 _spawnPoint;

    public Team _team;

    [SerializeField]
    private SpawnManager _enemySpawnManager;

    [SerializeField]
    private FieldObject _castle;

    public Vector3 GetCastlePosition() { return _castle.transform.position; }

    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void OnSpawn(int index)
    {
        UnitController unitCtrl = new UnitController();

        // 

        unitCtrl._enemyCastlePosition = _enemySpawnManager.GetCastlePosition();

        unitCtrl._status = _team.GetUnit(index);

        unitCtrl._status.UpdateItems();
        UnitModelManager.UpdateModel(unitCtrl.gameObject, unitCtrl._status._equipedItems);
    }

    #region Private Function

    #endregion
}

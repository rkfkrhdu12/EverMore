using System.Collections.Generic;
using UnityEngine;

//오브젝트 제거할 때 사용
//ex -> AddDeleteObject(GameObject);
public class DeleteObjectSystem
{
    #region init

    private static readonly Dictionary<string, GameObject> _deleteObjList = new Dictionary<string, GameObject>();

    private static GameObject _trashCan;

    private static int _count = 0;

    #endregion

    #region public Funtion

    /// <summary>
    /// Input GameObject(삭제할 오브젝트) 
    /// </summary>
    /// <param name="gObject">삭제할 오브젝트</param>
    // GameObject를 넣어두면 SetActive(false), SetParent(null) 실행 후 실제로 삭제하진 않음.
    public static void AddDeleteObject(GameObject gObject, string name = "")
    {
        if (_trashCan == null) { _trashCan = new GameObject(); }

        gObject?.transform.SetParent(_trashCan != null ? _trashCan.transform : null);
        gObject?.SetActive(false);

        if (gObject != null) _deleteObjList.Add(name + (++_count).ToString(), gObject);
    }

    #endregion
}

using System.Collections.Generic;
using UnityEngine;

//오브젝트 제거할 때 사용
//ex -> AddDeleteObject(GameObject);
public class DeleteObjectSystem
{
    #region init

    private static readonly Queue<GameObject> _deleteObjList = new Queue<GameObject>();

    #endregion

    #region public Funtion

    /// <summary>
    /// Input GameObject(삭제할 오브젝트) 
    /// </summary>
    /// <param name="gObject">삭제할 오브젝트</param>
    // GameObject를 넣어두면 SetActive(false), SetParent(null) 실행 후 실제로 삭제하진 않음.
    public static void AddDeleteObject(GameObject gObject)
    {
        gObject?.transform.SetParent(null);
        gObject?.SetActive(false);

        if (gObject != null) _deleteObjList.Enqueue(gObject);
    }
    
    #endregion
}

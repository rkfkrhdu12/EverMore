using System.Collections.Generic;
using UnityEngine;

//오브젝트 제거할 때 사용
//ex -> AddDeleteObject(GameObject);
public class DeleteObjectManager
{
    #region init

    private static readonly Queue<GameObject> _deleteObjList = new Queue<GameObject>();

    #endregion

    #region public Funtion

    /// <summary>
    /// Input GameObject(삭제할 오브젝트) 
    /// </summary>
    /// <param name="gObject">삭제할 오브젝트</param>
    // GameObject를 넣어두면 SetActive(false), SetParent(null) 실행 후, LateUpdate() 에서 삭제
    public static void AddDeleteObject(GameObject gObject)
    {
        gObject?.transform.SetParent(null);
        gObject?.SetActive(false);

        if (gObject != null) _deleteObjList.Enqueue(gObject);
    }

    /// <summary>
    /// 삭제 예정인 오브젝트 수 return
    /// </summary>
    /// <returns></returns>
    public static int getCount() => //삭제해야할 오브젝트의 개수 return
        _deleteObjList.Count;
    
    //GameManager에서만 사용
    public static GameObject getDequeue() => //오브젝트를 제거하고 리턴합니다.
        _deleteObjList.Count <= 0 ? null : _deleteObjList.Dequeue();
    
    #endregion
}

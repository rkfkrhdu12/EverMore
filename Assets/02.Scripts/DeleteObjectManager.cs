using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 제거할때 사용 AddDeleteObject(GameObject);
/// </summary>
public class DeleteObjectManager
{
    static Queue<GameObject> _deleteObjList = new Queue<GameObject>();

    /// <summary>
    /// GameObject 를 넣어두면 SetActive(false), SetParent(null) 다한 후 LateUpdate() 에서 삭제
    /// </summary>
    /// <param name="gObject"></param>
    public static void AddDeleteObject(GameObject gObject)
    {
        if(null == gObject) { return; }

        gObject.transform.SetParent(null);
        gObject.SetActive(false);
        _deleteObjList.Enqueue(gObject);
    }

    #region Private Function
    // Public 이지만 GameManager 에서만 사용함.

    /// <summary>
    /// GameManager에서만 사용 | 삭제해야할 오브젝트 return
    /// </summary>
    /// <returns></returns>
    public GameObject Dequeue()
    {
        if (0 >= _deleteObjList.Count) return null;

        return _deleteObjList.Dequeue();
    }

    /// <summary>
    /// 삭제해야할 오브젝트의 갯수 return
    /// </summary>
    /// <returns></returns>
    public int GetCount()
    {
        return _deleteObjList.Count;
    } 
    #endregion
}

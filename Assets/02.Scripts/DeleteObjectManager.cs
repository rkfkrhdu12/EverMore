using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectManager
{
    static Queue<GameObject> _deleteObjList = new Queue<GameObject>();

    public static void AddDeleteObject(GameObject gObject)
    {
        if(null == gObject) { return; }

        gObject.transform.SetParent(null);
        gObject.SetActive(false);
        _deleteObjList.Enqueue(gObject);
    }

    public GameObject Dequeue()
    {
        if (0 >= _deleteObjList.Count) return null;

        return _deleteObjList.Dequeue();
    }

    public int GetCount()
    {
        return _deleteObjList.Count;
    }
}

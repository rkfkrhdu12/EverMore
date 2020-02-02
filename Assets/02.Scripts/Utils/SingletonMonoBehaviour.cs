using UnityEngine;
using System.Collections;

// DontDestroy Class 랑 성격이 비슷함.
// 해당 Scene 에서만 하나만 존재할 수 있다.
// 다른 Scene 으로 넘어가면 삭제된다.
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    static public T Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Instance == (T)this)
        {
            OnStart();
        }
    }

    virtual protected void OnAwake()
    {
    }
    
    virtual protected void OnStart()
    {
    }

}


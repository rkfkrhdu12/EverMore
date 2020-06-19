using UnityEngine;
using UnityEngine.Events;

public class SubUnityEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent CS = null;

    public void StartCS() => 
        CS?.Invoke();
}

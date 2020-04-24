using UnityEngine;
using UnityEngine.Events;

public class SubUnityEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent CS;

    public void StartCS() => 
        CS?.Invoke();
}

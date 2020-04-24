using System;
using UnityEngine;

public class info : MonoBehaviour
{
    private RectTransform _rect;

    private void Awake() => 
        _rect = GetComponent<RectTransform>();
    
    private void Update() => 
        _rect.position = Input.mousePosition;
}

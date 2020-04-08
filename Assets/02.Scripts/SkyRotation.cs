using System;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{
    [SerializeField, Tooltip("회전 속도")]
    private float Speed = 1.3f;

    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private void Update()
    {
        RenderSettings.skybox.SetFloat(Rotation, Time.time * Speed);
    }
}

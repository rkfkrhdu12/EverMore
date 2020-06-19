using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    private float _maxXPos = 15.0f;
    [SerializeField]
    private float _moveXSpeed = 15.0f;

    [SerializeField]
    private float _mouseSensitivity = 6.0f;

    private void Update()
    {
        float moveMouseX = Input.GetAxis("Mouse X");
        if (0 == moveMouseX) { return; }

        float moveDirection = -moveMouseX;

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            float x = transform.localPosition.x + moveDirection * _moveXSpeed;
            x = Mathf.Clamp(x, -_maxXPos, _maxXPos);

            Vector3 target = new Vector3(x, 0, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * _mouseSensitivity);
        }

    }
}

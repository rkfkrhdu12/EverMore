using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform _transform;

    private void Awake()
    {
        _transform = gameObject.transform;
    }

    // Test 1
    // 카메라를 움직이게 하거나 움직임을 제한한다.
    float _prevMousePositionX;
    const float _moveSpeed = .5f;
    public bool _isMoveRightDrag = true;
    void Test1CameraUpdate()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            float moveDistance = Input.GetAxis("Mouse X") * (_isMoveRightDrag ? 1 : -1);
            float movePositionX = _transform.position.x + moveDistance;

            if (movePositionX == Mathf.Clamp(movePositionX, 0.0f, 20.0f))
            {
                // 카메라의 이동은 게임이 PAUSE 되어도 움직일수 있어야한다.(Test 기준)
                _transform.Translate(moveDistance * _moveSpeed, 0, 0);
            }
        }
    }

    public void Update()
    {
        Test1CameraUpdate();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윤용우
public class CameraManager : SingletonMonoBehaviour<CameraManager>
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

            _transform.position = new Vector3(Mathf.Clamp(transform.position.x + (moveDistance * _moveSpeed), 0.0f, 20.0f), transform.position.y, transform.position.z);
        }
    }

    public void Update()
    {
        Test1CameraUpdate();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윤용우
public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    private void Awake()
    {
        //transform.localPosition = Vector3.zero;
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

            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x + (moveDistance * _moveSpeed), 0.0f, 30f),
                transform.localPosition.y, transform.localPosition.z);
        }
    }

    public void Update()
    {
        Test1CameraUpdate();
    }

}

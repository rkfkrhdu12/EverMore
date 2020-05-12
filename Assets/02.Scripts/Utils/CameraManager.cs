using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    private float _maxXPos = 15.0f;
    [SerializeField]
    private float _moveXSpeed = 15.0f;
    float _moveMouseX;

    //public void OnMouseMoveEnter(float moveDirection)
    //{
    //    Debug.Log("Enter");
    //    _moveMouseX = moveDirection;
    //}

    //public void OnMouseMoveExit()
    //{
    //    Debug.Log("Exit");
    //    _moveMouseX = 0;
    //}


    private void Update()
    {
        //if (_moveMouseX != 0)
        //{
        //    float x = Mathf.Clamp(transform.localPosition.x + _moveMouseX * _moveXSpeed, -_maxXPos, _maxXPos);
        //    Vector3 target = new Vector3(x, 0, 0);

        //    transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 4.0f);
        //}

        float moveMouseX = Input.GetAxis("Mouse X");
        if (0 == moveMouseX) { return; }

        float flipValue = Mathf.Abs(1.0f / moveMouseX);
        float moveDirection = -moveMouseX;

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            float x = transform.localPosition.x + moveDirection * _moveXSpeed;
            x = Mathf.Clamp(x, -_maxXPos, _maxXPos);

            Vector3 target = new Vector3(x, 0, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 2.0f);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    public int CameraMode = 1;
    public bool _isMoveRightDrag = true;

    private float _maxXPos = 15.0f;
    [SerializeField]
    private float _moveXSpeed = 5.0f;

    [SerializeField]
    private GameObject[] _CMCams;

    private int _curCamNum;

    void UpdateCamera(int camNum)
    {
        for (int i = 0; i < _CMCams.Length; ++i)
        {
            _CMCams[i].SetActive(camNum == i ? true : false);
        }
    }

    private void Start()
    {
        switch(CameraMode)
        {
            case 1:
            _curCamNum = 0;
            UpdateCamera(_curCamNum);
            break;
            
        }
    }

    private void Update()
    {
        float moveMouseX = Input.GetAxis("Mouse X");
        if (0 == moveMouseX) { return; }

        float flipValue = Mathf.Abs(1.0f / moveMouseX);
        float moveDirection = (moveMouseX * (_isMoveRightDrag ? flipValue : -flipValue));

        switch (CameraMode)
        {
            case 1:
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                if (_curCamNum + (int)moveDirection >= _CMCams.Length || _curCamNum + (int)moveDirection < 0) { return; }

                _curCamNum += (int)moveDirection;
                UpdateCamera(_curCamNum);
            }
            break;
            case 2:
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                float x = transform.localPosition.x + moveDirection * _moveXSpeed;
                x = Mathf.Clamp(x, -_maxXPos, _maxXPos);

                Vector3 target = new Vector3(x , 0, 0);

                transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 4.0f);
            }
            break;
        }
    }
}

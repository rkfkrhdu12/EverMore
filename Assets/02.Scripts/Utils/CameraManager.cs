using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윤용우
public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
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
        _curCamNum = 0;
        UpdateCamera(_curCamNum);
    }

    public bool _isMoveRightDrag = true;
    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            float moveMouseX = Input.GetAxis("Mouse X");
            if(0 == moveMouseX) { return; }

            float flipValue = Mathf.Abs(1.0f / moveMouseX);
            int moveDirection = (int)(moveMouseX * (_isMoveRightDrag ? flipValue : -flipValue));

            if(_curCamNum + moveDirection >= _CMCams.Length || _curCamNum + moveDirection < 0) { return; }

            _curCamNum += moveDirection;
            UpdateCamera(_curCamNum);
        }
    }

}

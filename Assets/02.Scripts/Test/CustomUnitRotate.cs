using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUnitRotate : MonoBehaviour
{
    [SerializeField] GameObject _teamObject;

    int _resetY = 160;

    bool _isRotate = false;
    void Update()
    {
        if(_isRotate)
        {
            _teamObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * -10, 0));
        }
        else
        {
            Vector3 targetPos = Vector3.zero - _teamObject.transform.rotation.eulerAngles;

            _teamObject.transform.Rotate(targetPos * Time.deltaTime);
        }
    }

    public void OnRotate() { _isRotate = true; }
    public void OffRotate() { _isRotate = false;  }
}

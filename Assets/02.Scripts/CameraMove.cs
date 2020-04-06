using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private bool MyView;

    private Vector3 left = new Vector3(23, 1, -41);
    private Vector3 right = new Vector3(-5, 1, -41);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MyView = !MyView;
        
        if (MyView)
            transform.position = Vector3.Lerp(transform.position, left, 3.5f*  Time.deltaTime);
        else 
            transform.position = Vector3.Lerp(transform.position, right, 3.5f*  Time.deltaTime);
    }
}

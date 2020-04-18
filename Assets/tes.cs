using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tes : MonoBehaviour
{
    public GameObject obj;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            obj.SetActive(true);
        
        if(Input.GetKeyDown(KeyCode.D))
            obj.SetActive(false);
    }
}

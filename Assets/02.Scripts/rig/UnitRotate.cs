using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitRotate : MonoBehaviour, IPointerDownHandler
{
    #region Show Inspector

    [SerializeField, Tooltip("오브젝트 회전 세기")]
    private float rotSpeed = 1.0f; //ADD

    [SerializeField, Tooltip("실제 유닛 오브젝트")]
    private Transform Units;

    #endregion

    #region Hide Inspector

    //회전을 허락할지 말지의 변수
    private bool isRotate;

    #endregion

    private void Update()
    {
        //회전 중에 유닛 미리보기 이미지 영역을 나갔을 때 : Not false
        if (!Input.GetMouseButton(0))
            isRotate = false;
        
        // 마우스가 클릭되지 않았다면 : return
        if (!Input.GetMouseButton(0)) return;

        //회전 권환이 없다면 : return
        if (!isRotate) return;

        float MouseX = Input.GetAxis("Mouse X");

        Units.Rotate(-Vector3.up * (rotSpeed * MouseX));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isRotate = true;
    }
}

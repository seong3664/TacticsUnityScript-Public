using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCam : MonoBehaviour
{
    private void FixedUpdate()
    {
        //transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
        Vector3 targetPosition = Camera.main.transform.position;
        targetPosition.y = transform.position.y;  // UI의 Y축 위치 고정
        transform.LookAt(targetPosition);         // 카메라 방향을 바라보게 회전
    }

}

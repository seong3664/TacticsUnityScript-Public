using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCamSet : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    public CinemachineVirtualCamera NomCam;
    public CinemachineVirtualCamera AimCam;
    private Coroutine lookTargetCoroutine;
    private void Awake()
    {
        GameManager = GameObject.Find("Game_Manager");
        AimCam = GameObject.Find("Aim_Cam").GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
       
        NomCam.enabled = true;
    }
    /// <summary>
    /// 선택한 유닛과 공격할 유닛을 받아
    /// 공격 시점을 설정함.
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="Target"></param>
    public void SetSelectUnitCam(Transform Unit, Transform Target)
    {
        //AimCam.transform.position = Unit.position + new Vector3(0f, 1.85f, -1.54f);
        AimCam.enabled = true;
        NomCam.enabled = false;
        Vector3 direction = Target.position - Unit.position;
        direction.y = 0; 
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        //Unit.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        if (lookTargetCoroutine != null)
        {
            StopCoroutine(lookTargetCoroutine);
        }

        lookTargetCoroutine = StartCoroutine(LookTarget(Unit, targetRotation));
        AimCam.Follow = Unit;
        AimCam.LookAt = Unit;   
    }
    IEnumerator LookTarget(Transform Unit,Quaternion TargetRot)
    {
        float R_Time = 0f;
        while (Unit.rotation != TargetRot)
        {
            R_Time += Time.deltaTime;
            Unit.rotation = Quaternion.Slerp(Unit.rotation, TargetRot, R_Time*0.25f);
            yield return null;
        }
        lookTargetCoroutine = null;
    }
    public void OffAtkCam(Transform Unit)
    {
        AimCam.enabled=false;
        NomCam.enabled = true;
        GameManager.transform.position = Unit.position;

    }

}

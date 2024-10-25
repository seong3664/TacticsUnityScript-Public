using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Enemy_Manager;

public class Enemy_Ctrl : MonoBehaviour, IEnemy_Ctrl
{
    MoveCtrl moveCtrl;
    Unit_Fire unitFire;

    Unit_AniCtrl AniCtrl;
    public event Action OnEnemyActionCompleted;

    GameObject EnemyAtkCam;
    CinemachineTargetGroup TargetGroup;

    private void Awake()
    {
        moveCtrl = GetComponent<MoveCtrl>();
        EnemyAtkCam = GameObject.Find("EnemyAtkLookCam");
        TargetGroup = transform.GetChild(1).GetComponent<CinemachineTargetGroup>();
    }
    private void Start()
    {
        EnemyAtkCam.SetActive(false);
    }
    public void EnemyMove(Transform Unit, List<A_Nodes> MoveNode)
    {
        AniCtrl = Unit.GetComponent<Unit_AniCtrl>();
        StartCoroutine(MoveEnemy(Unit, MoveNode));
    }

    private IEnumerator MoveEnemy(Transform Unit, List<A_Nodes> MoveNode)
    {
        Debug.Log("이동 시작");
        AniCtrl.MoveAniSet(true);
        yield return moveCtrl.MoveNode(Unit, MoveNode);
        AniCtrl.MoveAniSet(false);
        yield return new WaitForSeconds(1f);
        Debug.Log("이동 끝나고 2초 대기후 대리자 호출 해야함");
        OnEnemyActionCompleted?.Invoke();
    }

    public void EnemyAtk(Transform Unit, Transform Target, UnitStat stat)
    {
        AniCtrl = Unit.GetComponent<Unit_AniCtrl>();
        TargetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
        TargetGroup.AddMember(Unit, 1, 2);
        TargetGroup.AddMember(Target, 1, 4);
        StartCoroutine(WaitEnemyATk(Unit, Target, stat));
    }
    IEnumerator WaitEnemyATk(Transform Unit, Transform Target, UnitStat stat)
    {
        unitFire = Unit.GetComponent<Unit_Fire>();
        EnemyAtkCam.SetActive(true);
        yield return new WaitForSeconds(1f);
        AniCtrl.AtkAniSet(true);
        unitFire.FireRay(Target, stat.stat.dmg);
        yield return new WaitForSeconds(0.5f);
        AniCtrl.AtkAniSet(false);
        yield return new WaitForSeconds(1f);
        EnemyAtkCam.SetActive(false);
        OnEnemyActionCompleted?.Invoke();
    }
}

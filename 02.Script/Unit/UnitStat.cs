using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using System;
public class UnitStat : MonoBehaviour
{

    private Stat Stat;
    public Stat stat
    { get; private set; }


    public Stat statPrefab;
    public int BulletCount;
    private void Awake()
    {
          turnManager.EndTurn += ResetActionPoints;
        stat = Instantiate(statPrefab); //중복을 피하기위해 동적할당함.
        stat.unit_Inspector = transform.GetChild(0).GetComponent<Unit_inspector_Ctrl>();
    }
    private void Start()
    {
        ResetActionPoints();
        BulletCount = 3;
    }

    private void ResetActionPoints()
    {
        stat.Action = 2;
    }
}


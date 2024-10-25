using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using System;
public class UnitStat : MonoBehaviour
{

    public Stat stat;
    public Stat statPrefab;
    private void Awake()
    {
          turnManager.EndTurn += ResetActionPoints;
        stat = Instantiate(statPrefab);
        stat.unit_Inspector = transform.GetChild(0).GetComponent<Unit_inspector_Ctrl>();
    }
    private void Start()
    {
        stat.Action = 2;
    }

    private void ResetActionPoints()
    {
        stat.Action = 2;
    }
}


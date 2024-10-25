using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using System;
using UnityEditor;
using static Equip_Inpo;
public class UnitStat : MonoBehaviour
{

    private Stat Stat;
    public Stat stat
    { get; private set; }

    public GameObject Vest;
    public GameObject Scope;
    public GameObject Muzzle;

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
    public void EquipOn(bool onoff,Equip_Inpo.EquipType equipType)
    {
        switch (equipType)
        {
            case EquipType.Hp:
                Vest.SetActive(onoff);
                break;
            case EquipType.aiming:
                Muzzle.SetActive(onoff);
                break;
            case EquipType.Crit:
                Scope.SetActive(onoff);
                break;
        }
    }
}


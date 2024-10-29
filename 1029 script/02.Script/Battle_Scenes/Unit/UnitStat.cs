using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using UnityEditor;
using static Equip_Inpo;
public class UnitStat : MonoBehaviour
{

    private Stat Stat;
    public Stat EnemyStat;
    public Stat stat
    { get;set; }

    public GameObject Vest;
    public GameObject Scope;
    public GameObject Muzzle;

 
    public int BulletCount;
    private void Awake()
    {
        if (EnemyStat != null)
            stat = Instantiate(EnemyStat);
        turnManager.EndTurn += ResetActionPoints;
    }
    private void Start()
    {
        if (transform.GetChild(0).TryGetComponent<Unit_inspector_Ctrl>(out var unitInspector))
        {
            stat.unit_Inspector = unitInspector;
        }

        BulletCount = 3;
    }

    private void ResetActionPoints()
    {
        stat.Action = 2;
    }
    public void UpdateEquipOnoff()
    {
        Vest.SetActive(stat.VestOnoff);
        Muzzle.SetActive(stat.MuzzleOnoff);       
        Scope.SetActive(stat.ScopeOnoff);
    }
}


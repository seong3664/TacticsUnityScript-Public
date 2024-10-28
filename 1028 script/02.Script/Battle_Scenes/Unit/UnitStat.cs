using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using UnityEditor;
using static Equip_Inpo;
public class UnitStat : MonoBehaviour
{

    private Stat Stat;
    public Stat stat
    { get;set; }

    public GameObject Vest;
    public GameObject Scope;
    public GameObject Muzzle;

 
    public int BulletCount;
    private void Awake()
    {
        if (stat == null)
        {
            stat = new Stat(3,Random.Range(4,6),Random.Range(5,7),2,Random.Range(60,80),10,10);
        }
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


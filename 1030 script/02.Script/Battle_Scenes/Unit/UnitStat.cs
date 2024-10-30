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
    { get { return Stat; }
        set { Stat = value; }
    }

    public GameObject Vest;
    public GameObject Scope;
    public GameObject Muzzle;

 
    public int BulletCount;
    private void Awake()
    {
        if (EnemyStat != null && GameManager.gamemaneger.isnewGame)
            stat = Instantiate(EnemyStat);
        turnManager.EndTurn += ResetActionPoints;
    }
    private void Start()
    {
        if(stat != null && stat.UnitCode == UnitCode.Player)
        {
            Debug.Log(stat.UnitCode);
            UpdateEquipOnoff();
        }
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


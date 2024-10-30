using States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitStatData
{
    public string unitCode;
    public int dmg;
    public int hp;
    public int movePoint;
    public int actionPoint;
    public int aiming;
    public int evasion;
    public int crit;
    public bool scopeOnoff;
    public bool vestOnoff;
    public bool muzzleOnoff;

    // Constructor to convert Stat to SerializableStatData
    public UnitStatData(Stat stat)
    {
        unitCode = stat.UnitCode.ToString();
        dmg = stat.dmg;
        hp = stat.Hp;
        movePoint = stat.MovePoint;
        actionPoint = stat.Action;
        aiming = stat.Aiming;
        evasion = stat.Evasion;
        crit = stat.Crit;
        scopeOnoff = stat.ScopeOnoff;
        vestOnoff = stat.VestOnoff;
        muzzleOnoff = stat.MuzzleOnoff;
    }
}
[Serializable]
public class UnitData
{
    public UnitStatData statData;
    public Vector3 position;
    public Quaternion rotation;
    public UnitData(Stat stat, Transform unitTransform)
    {
        statData = new UnitStatData(stat);
        position = unitTransform.position;
        rotation = unitTransform.rotation;
    }
}
[Serializable]
public class UnitSaveData
{
    public string sceneName;
    public List<UnitData> units = new List<UnitData>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_crouch : MonoBehaviour
{
    UnitStat Unitstat;
    int CountTurn;
    private void Start()
    {
        CountTurn = 0;
        turnManager.EndTurn += ResetUnitstat;
    }
    public void CoruchUnit(Transform SelectUnit)
    {
        Unitstat = SelectUnit.GetComponent<UnitStat>();
        Unitstat.stat.Evasion += 20;
        Unitstat.stat.Action -= 2;
    }
    private void ResetUnitstat()
    {
        CountTurn++;
        if (CountTurn > 1 && Unitstat !=null)
        {
            Unitstat.stat.Evasion -= 20;
            CountTurn = 0;
        }
    }
}

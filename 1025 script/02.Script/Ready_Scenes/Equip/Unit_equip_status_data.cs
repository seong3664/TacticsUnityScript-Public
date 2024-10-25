using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unitequipstatusdata
{
    public List<Unit_Equip_Slot> Equipslots = new List<Unit_Equip_Slot>();
    public List<Unit_Equip_Slot> Invenslots = new List<Unit_Equip_Slot>();

    public Unitequipstatusdata(List<Unit_Equip_Slot> equipslots, List<Unit_Equip_Slot> invenslots)
    {
        Equipslots = equipslots;
        Invenslots = invenslots;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType { Equip,Inven }
public class Unit_Equip_Slot : MonoBehaviour
{
    public SlotType slotType;
    public bool haveItem;
    private void Start()
    {
        if (slotType == SlotType.Equip)
        {
            haveItem = false;
        }
        else
        {
            haveItem = true;
        }
    }
    public void AddItem(Equip_Inpo equip_Inpo)
    {
        if (slotType == SlotType.Equip)
        {
            UnitSet_Manager.unitsetmanager.EquipUnitStat(equip_Inpo, true);
            haveItem = true;
        }
        //else if (slotType == SlotType.Inven)
        //{
        //    UnitSet_Manager.unitsetmanager.EquipUnitStat(equip_Inpo, false);
          
        //}
    }
    public void OutItem(Equip_Inpo equip_Inpo)
    {
        if (slotType == SlotType.Equip)
        {
            UnitSet_Manager.unitsetmanager.EquipUnitStat(equip_Inpo, false);
            haveItem = false;

        }
    }
}

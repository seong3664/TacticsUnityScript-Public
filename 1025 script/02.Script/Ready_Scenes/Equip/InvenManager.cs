using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenManager : MonoBehaviour
{
    Dictionary<int, Unitequipstatusdata> UnitInven = new Dictionary<int, Unitequipstatusdata>();
    private List<Unit_Equip_Slot> EquipSlotList = new List<Unit_Equip_Slot>();
    private List<Unit_Equip_Slot> InvenSlotList = new List<Unit_Equip_Slot>();
    private int currentUnitnumber;
    private Unitequipstatusdata currentUnitInventory;
    private void Awake()
    {
       EquipSlotList = new List<Unit_Equip_Slot>(transform.GetChild(2).GetChild(0).GetComponentsInChildren<Unit_Equip_Slot>());
       InvenSlotList = new List<Unit_Equip_Slot>(transform.GetChild(2).GetChild(1).GetComponentsInChildren<Unit_Equip_Slot>());
    }
    private void Start()
    {
        currentUnitnumber = UnitSet_Manager.unitsetmanager.unitnumber;
        for (int i = 0; i < 3; i++)
        {
            UnitInven[i] = new Unitequipstatusdata(EquipSlotList, InvenSlotList);
        }
        
    }
    void ChangeUnit()
    {
        currentUnitnumber = UnitSet_Manager.unitsetmanager.unitnumber;
    }    
}

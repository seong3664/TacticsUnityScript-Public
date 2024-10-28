using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equip_Inpo
{
    public enum EquipType { Hp, movePoint, aiming,Crit }
    public EquipType equipType;
    public string name;
    public string desc;
    public int[] value;
}

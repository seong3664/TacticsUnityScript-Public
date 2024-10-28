using States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static Equip_Inpo;

public class UnitSet_Manager : MonoBehaviour
{
    static UnitSet_Manager unitset_Manager;
    public Stat statprefab;
    [SerializeField]
    public List<Stat> unitstat;
    public UnitStat Playerbase;
    public int unitnumber = 0;

    TMP_Text UnitNumberTxt;
    [SerializeField]
    Button[] ChangeUnitBtns;


    [SerializeField]
    Dictionary<int, List<Transform>> UnitInven = new Dictionary<int, List<Transform>>();
    [SerializeField]
    private List<Transform> itemList = new List<Transform>();
    [SerializeField]
    private List<Transform> EquipSlotList = new List<Transform>();
    private int currentUnitnumber;
    public static UnitSet_Manager unitsetmanager
    {
        private set
        {
            if (unitset_Manager == null)
                unitset_Manager = value;
            else
                Destroy(value.gameObject);
        }
        get
        {
            return unitset_Manager;
        }
    }
    private void Awake()
    {
        unitsetmanager = this;
        Playerbase = GameObject.Find("Player Base").GetComponent<UnitStat>();
       

 
        var equips = transform.GetComponentsInChildren<Equip>();

        foreach (var equip in equips)
        {
            itemList.Add(equip.transform);
        }

        UnitNumberTxt = transform.GetChild(4).GetComponent<TMP_Text>();
        ChangeUnitBtns = transform.GetChild(3).GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        foreach (Button btn in ChangeUnitBtns)
        {
            btn.onClick.RemoveAllListeners();
        }

        InitializeUnits();

        ChangeUnitBtns[0].onClick.AddListener(() => ChangeUnit(1));
        ChangeUnitBtns[1].onClick.AddListener(() => ChangeUnit(-1));
        currentUnitnumber = unitsetmanager.unitnumber;
    }

    private void InitializeUnits()
    {
        for (int i = 0; i < 4; i++)
        {
            unitstat[i] = Instantiate(statprefab);
            UnitInven[i] = new List<Transform>(itemList.Count);
            Debug.Log(itemList.Count);
            foreach (var item in itemList)
            {
                UnitInven[i].Add(item.parent);
            }
        }
        EquipSlotList = UnitInven[unitnumber];
        Playerbase.stat = unitstat[unitnumber];
        Debug.Log(unitstat.Count);
        
    }

    public void EquipUnitStat(Equip_Inpo equip_Inpo, bool Equip)
    {
        switch (equip_Inpo.equipType)
        {
            case EquipType.Hp:
                if (Equip && unitstat[unitnumber].VestOnoff == false)
                {
                    unitstat[unitnumber].Hp += equip_Inpo.value[0];
                    unitstat[unitnumber].MovePoint += equip_Inpo.value[1];
                    unitstat[unitnumber].VestOnoff = true;
                }
                else if(!Equip && unitstat[unitnumber].VestOnoff == true)
                {
                    unitstat[unitnumber].Hp -= equip_Inpo.value[0];
                    unitstat[unitnumber].MovePoint -= equip_Inpo.value[1];
                    unitstat[unitnumber].VestOnoff = false;
                }
                break;
            case EquipType.aiming:
                if (Equip && unitstat[unitnumber].MuzzleOnoff == false)
                {
                    unitstat[unitnumber].Aiming += equip_Inpo.value[0];
                    unitstat[unitnumber].MuzzleOnoff = true;
                }
                else if (!Equip && unitstat[unitnumber].MuzzleOnoff == true)
                {
                    unitstat[unitnumber].Aiming -= equip_Inpo.value[0];
                    unitstat[unitnumber].MuzzleOnoff = false;
                }
                break;
            case EquipType.movePoint:
                if (Equip)
                {
                    unitstat[unitnumber].MovePoint += equip_Inpo.value[0];
                }
                else
                {
                    unitstat[unitnumber].MovePoint -= equip_Inpo.value[0];
                }
                break;
            case EquipType.Crit:
                if (Equip && unitstat[unitnumber].ScopeOnoff == false)
                {
                    unitstat[unitnumber].Crit += equip_Inpo.value[0];
                    unitstat[unitnumber].ScopeOnoff = true;
                }
                else if (!Equip && unitstat[unitnumber].ScopeOnoff == true)
                {
                    unitstat[unitnumber].Crit -= equip_Inpo.value[0];
                    unitstat[unitnumber].ScopeOnoff = false;
                }
                break;
        }
        for (int i = 0; i < EquipSlotList.Count; i++)
        {
            EquipSlotList[i] = itemList[i].parent;
            
        }
        UnitInven[unitnumber] = EquipSlotList;
        Playerbase.UpdateEquipOnoff();
    }
    public void ChangeUnit(int num)
    {
        unitnumber = Mathf.Clamp(unitnumber + num, 0, 3);
        EquipSlotList = UnitInven[unitnumber];
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].parent != EquipSlotList[i])
            {
                itemList[i].SetParent(EquipSlotList[i]);
            }
        }
        UnitNumberTxt.text = $"Unit {unitnumber}";
        Playerbase.stat = unitstat[unitnumber];
        Playerbase.UpdateEquipOnoff();

    }
}


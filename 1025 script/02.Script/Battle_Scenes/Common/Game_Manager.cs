using States;
using System.Collections;
using System.Collections.Generic;
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
    private Stat[] unitstat;
    public UnitStat Playerbase;
    public int unitnumber = 0;

    TMP_Text Unitnumbetxt;
    [SerializeField]
    Button[] ChangeUntbtn;
    
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
        DontDestroyOnLoad(gameObject);
        Unitnumbetxt = GameObject.Find("Ready_UI").transform.GetChild(4).GetComponent<TMP_Text>();
        ChangeUntbtn = GameObject.Find("Ready_UI").transform.GetChild(3).GetComponentsInChildren<Button>();
    }
    private void Start()
    {
        foreach (Button btn in ChangeUntbtn)
        {
            btn.onClick.RemoveAllListeners();
            
        }
        for(int i =0;i < 4;i++) 
        {
            unitstat[i] = Instantiate(statprefab);
            
        }
        Playerbase.stat = unitstat[unitnumber];
        ChangeUntbtn[0].onClick.AddListener(() => ChangeUnit(1));
        ChangeUntbtn[1].onClick.AddListener(() => ChangeUnit(-1));
        
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
        Playerbase.UpdateEquipOnoff();
    }
    public void ChangeUnit(int num)
    {
        unitnumber += num;
        unitnumber = Mathf.Clamp(unitnumber, 0, 3);
        Unitnumbetxt.text = $"Unit {unitnumber}";
        Playerbase.stat = unitstat[unitnumber];
        Playerbase.UpdateEquipOnoff();

    }
}


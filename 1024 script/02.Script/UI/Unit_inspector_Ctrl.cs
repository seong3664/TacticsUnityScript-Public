using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_inspector_Ctrl : MonoBehaviour
{
    UnitStat unitStat;
    [SerializeField]
    Image[] Hp_sell;
    [SerializeField]
    Image AP_Ui;

    private void Awake()
    {
        unitStat = transform.parent.GetComponent<UnitStat>();
        AP_Ui = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        Hp_sell = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Image>();
    }
    public void UpdateUInspector()
    {
        AP_Ui.fillAmount = (float)(unitStat.stat.Action / 2f);

        for (int i = 0; i < Hp_sell.Length; i++)
        {
            Hp_sell[i].enabled = true;
            if(i > unitStat.stat.Hp-1)
            {
                Hp_sell[i].enabled = false;
            }
        }
        if (unitStat.stat.Hp < 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}

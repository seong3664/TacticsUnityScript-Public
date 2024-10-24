using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Equip_Inpo;

public class Game_Manager : MonoBehaviour
{
    static Game_Manager game_Manager;
    UnitStat[] unitstat;
    public static Game_Manager gamemanager
    {
        private set
        {
            if (game_Manager == null)
                game_Manager = value;
            else
                Destroy(value.gameObject);
        }
        get
        {
            return game_Manager;
        }
    }
    private void Awake()
    {
        gamemanager = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EquipUnitStat(Equip_Inpo equip_Inpo,int Unitnumber, bool Equip)
    {
        switch (equip_Inpo.equipType)
        {
            case EquipType.Hp:
                if (Equip)
                {
                    unitstat[Unitnumber].stat.Hp += equip_Inpo.value[0];
                    unitstat[Unitnumber].stat.MovePoint += equip_Inpo.value[1];
                }
                else
                {
                    unitstat[Unitnumber].stat.Hp -= equip_Inpo.value[0];
                    unitstat[Unitnumber].stat.MovePoint -= equip_Inpo.value[1];
                }
                unitstat[Unitnumber].EquipOn(Equip, EquipType.Hp);
                break;
            case EquipType.aiming:
                if (Equip)
                {
                    unitstat[Unitnumber].stat.Aiming += equip_Inpo.value[0];
                }
                else
                {
                    unitstat[Unitnumber].stat.Aiming -= equip_Inpo.value[0];
                }
                unitstat[Unitnumber].EquipOn(Equip, EquipType.aiming);
                break;
            case EquipType.movePoint:
                if (Equip)
                {
                    unitstat[Unitnumber].stat.MovePoint += equip_Inpo.value[0];
                }
                else
                {
                    unitstat[Unitnumber].stat.MovePoint -= equip_Inpo.value[0];
                }
                break;
            case EquipType.Crit:
                if (Equip)
                {
                    unitstat[Unitnumber].stat.Crit += equip_Inpo.value[0];
                }
                else
                {
                    unitstat[Unitnumber].stat.Crit -= equip_Inpo.value[0];
                }
                unitstat[Unitnumber].EquipOn(Equip, EquipType.Crit);
                break;
        }
    }
  }


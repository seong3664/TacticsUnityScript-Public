using States;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    IEnemy_Select enemy_select;
    IEnemy_Ai enemy_ai;
    IEnemy_Ctrl enemy_ctrl;

    Transform SelectEnemy;
    UnitStat SelectEnemy_stat;
    Unit_AniCtrl AniCtrl;

   
    private void Awake()
    {
        enemy_select = GetComponent<IEnemy_Select>();
        enemy_ai = GetComponent<IEnemy_Ai>();
        enemy_ctrl = GetComponent<IEnemy_Ctrl>();
        AniCtrl = GetComponent<Unit_AniCtrl>();

        turnManager.PlayerEndTurn += SetSelectEnemy;
        enemy_ctrl.OnEnemyActionCompleted += SetSelectEnemy;
    }
   
    void SetSelectEnemy()
    {
        if(SelectEnemy == null)
        {
            SelectEnemy = enemy_select.SelectcanMoveEnemy();
            if(SelectEnemy != null)
            {
                SelectEnemy_stat = SelectEnemy.GetComponent<UnitStat>();
            }
        }
        if(SelectEnemy != null)
        {
            
            if (SelectEnemy_stat.stat.Action > 0)
            {
                if (enemy_ai.EnemyMoveorAtk(SelectEnemy))
                {

                    List<A_Nodes> MoveNode = enemy_ai.EnemyMove(SelectEnemy, SelectEnemy_stat);
                    enemy_ctrl.EnemyMove(SelectEnemy, MoveNode);
                    SelectEnemy_stat.stat.Action--;
                }
                else
                {
                   Transform Target = enemy_ai.EnemyAtk(SelectEnemy);
                    enemy_ctrl.EnemyAtk(SelectEnemy,Target,SelectEnemy_stat);
                    //SelectEnemy_stat.stat.Action--; 
                }
            }
            else
            {
                SelectEnemy = null;
                SetSelectEnemy();
            }
        }
       
    }
}

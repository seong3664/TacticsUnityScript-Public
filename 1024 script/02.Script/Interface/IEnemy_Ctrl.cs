using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy_Ctrl
{
    public event System.Action OnEnemyActionCompleted;
    void EnemyMove(Transform Unit,List<A_Nodes> MoveNode);
    void EnemyAtk(Transform Unit,Transform Target,UnitStat stat);
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Enemy_Select : MonoBehaviour,IEnemy_Select
{
    Transform Cam;
    A_Grid grid;
 
   [SerializeField] LayerMask EnemyLayer;
    [SerializeField] GameObject[] EnemyList;
    [SerializeField] UnitStat[] EnemyListStat;

    Transform EnemyseletUnitUI;
    protected Transform SelectEnemy { get; private set; } // 외부에서 접근 가능하게 함

    

    private void Awake()
    {
        grid = GameObject.Find("Grid_Manager").GetComponent<A_Grid>();
        EnemyLayer = LayerMask.NameToLayer("Enemy");
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyseletUnitUI = transform.GetChild(0).GetComponent<Transform>();
        EnemyListStat = new UnitStat[EnemyList.Length];
    }
    private void Start()
    {
        Cam = GameObject.Find("Game_Manager").GetComponent<Transform>();
        Vector3 boxSize = new Vector3(grid.gridWorldSize.x, 1.0f, grid.gridWorldSize.y);
        for (int i = 0; i < EnemyList.Length; i++)
        {
            EnemyListStat[i] = EnemyList[i].GetComponent<UnitStat>();
        }
        EnemyseletUnitUI.gameObject.SetActive(false);
        
    }
    public Transform SelectcanMoveEnemy()
    {
        for (int i = 0; i < EnemyList.Length; i++)
        {
            if (EnemyListStat[i].stat.Action > 0)
            {
                Debug.Log(EnemyListStat[i].stat.Action);
                SelectEnemy = EnemyList[i].transform;
                Cam.position = SelectEnemy.position;
                return SelectEnemy;
            }

        }
        turnManager.TurnManager.EndEnemyturn();
        return null;
    }
    

}


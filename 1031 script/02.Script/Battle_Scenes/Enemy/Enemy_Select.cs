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
    [SerializeField] List<GameObject> EnemyList;
    [SerializeField] List<UnitStat> EnemyListStat;

    Transform EnemyseletUnitUI;
    protected Transform SelectEnemy { get; private set; } // 외부에서 접근 가능하게 함

    

    private void Awake()
    {
        grid = GameObject.Find("Grid_Manager").GetComponent<A_Grid>();
        EnemyLayer = LayerMask.NameToLayer("Enemy");
       
        EnemyseletUnitUI = transform.GetChild(0).GetComponent<Transform>();
        
    }
    private void Start()
    {
        Cam = GameObject.Find("Turn_Manager").GetComponent<Transform>();
        Vector3 boxSize = new Vector3(grid.gridWorldSize.x, 1.0f, grid.gridWorldSize.y);
       
        EnemyseletUnitUI.gameObject.SetActive(false);
        
    }
    public Transform SelectcanMoveEnemy()
    {
        EnemyList = GameManager.gamemaneger.EnemyList;
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyListStat.Add(EnemyList[i].GetComponent<UnitStat>());
        }
        for (int i = 0; i < EnemyList.Count; i++)
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


using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class MoveCtrl : MonoBehaviour
{   
    private float moveSpeed = 10f;
    
    Unit_AniCtrl Unitani;
    private bool isMove = true;
    private Transform Cam;
   [SerializeField] SetNode setnode;
    public bool IsMove
    {get { return isMove; } }

    private void Start()
    {
        Cam =GameObject.Find("Game_Manager").GetComponent<Transform>();
    }
    public IEnumerator MoveNode(Transform selectUnit, List<A_Nodes> path)
    {
        setnode = selectUnit.GetComponent<SetNode>();
        isMove = false;

        setnode.OutNode();
        foreach (A_Nodes node in path)
        {
            Vector3 targetPosition = node.WorldPos;
           
            // 타겟 위치까지 이동
            while (Vector3.Distance(selectUnit.position, targetPosition) > 0.1f)
            {
                //Debug.Log("제대로 이동하면 여러번 출력될것");
                Vector3 Targetdist = targetPosition - selectUnit.position;
                selectUnit.rotation = Quaternion.LookRotation(new Vector3(Targetdist.x, 0, Targetdist.z));
                selectUnit.position = Vector3.MoveTowards(selectUnit.position, targetPosition, moveSpeed * Time.deltaTime);
               yield return null;
                Cam.position = selectUnit.position;
               
            }

        }
        setnode.SetInNode();
        isMove = true;

    }
    //IEnumerator Move(Transform selectUnit, List<A_Nodes> path)
    //{
    //    isMove = false;
      
    //}
  
    

}

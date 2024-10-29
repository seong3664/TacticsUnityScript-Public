using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNode : MonoBehaviour
{
    public A_Grid grid;
   [SerializeField] A_Nodes NowNode;
    Unit_AniCtrl AniCtrl;
    private void Awake()
    {
        grid = GameObject.Find("Grid_Manager").GetComponent<A_Grid>();
        AniCtrl = GetComponent<Unit_AniCtrl>();
    }
    private void Start()
    {
        transform.position = grid.GetNodeFromWorldPoint(transform.position).WorldPos;
        SetInNode();
    }
    public void SetInNode()
    {
      NowNode = grid.GetNodeFromWorldPoint(transform.position);
      NowNode.Iswalkable = false;
        if (NowNode.Nodetype == NodeType.SemiCover || NowNode.Nodetype == NodeType.Cover)
        {
            AniCtrl.CoverKneeSet(true);
        }
    }
    public void OutNode()
    {
        Debug.Log("두번 호출?");
        NowNode.Iswalkable = true;
        AniCtrl.CoverKneeSet(false);
        NowNode = null;
    }
}

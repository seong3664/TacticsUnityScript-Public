using System.Collections;
using System.Collections.Generic;
using System.IO;
using TreeEditor;
using UnityEngine;

public class A_Grid : MonoBehaviour
{

    Transform Ground;
    LayerMask UnWalkableMask;
    LayerMask CoverMask;
    LayerMask SemiCoverMask;

    public Vector2 gridWorldSize;


    public float nodeRadius;
    public List<A_Nodes> reachableNodes = new List<A_Nodes>();   //�׸��� ���ﶧ ���� �����
    List<A_Nodes> UnWalkableNode = new List<A_Nodes>();
    A_Nodes[,] node;
    public List<A_Nodes> path;
    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;
   
    private void Awake()
    {
        Ground = GameObject.Find("Ground").GetComponent<Transform>();
        gridWorldSize = new Vector2(Ground.localScale.x, Ground.localScale.z) * 10;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //���� ����
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);  //���� ����
        UnWalkableMask = 1 << 6 | 1 << 10 | 1 << 11;
        CoverMask = 1 << 10;
        SemiCoverMask = 1 << 11;

        CreateGird();
    }
    void CreateGird()
    {
        node = new A_Nodes[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        Vector3 worldPoint;
        //List<A_Nodes> CoverNode;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x*nodeDiameter+nodeRadius)+Vector3.forward * (y*nodeDiameter+nodeRadius);
                bool walkable = !(Physics.CheckBox(worldPoint,Vector3.one*nodeRadius*0.9f,Quaternion.identity,UnWalkableMask));
                node[x, y] = new A_Nodes(walkable,worldPoint,x,y);
                if (node[x, y].Iswalkable == false)
                {
                    UnWalkableNode.Add(node[x, y]);
                }
            }
        }
        CheckCover();
    }
    void CheckCover()
    {
        foreach(A_Nodes node in UnWalkableNode)
        {
            if (Physics.CheckBox(node.WorldPos, Vector3.one * nodeRadius * 0.9f, Quaternion.identity, CoverMask))
            {
               List<A_Nodes> CoverinNode = GetNeighbours(node, false);
               foreach(A_Nodes coverinnode in CoverinNode)
                {
                    coverinnode.Nodetype = NodeType.Cover;
                }
            }
            else if (Physics.CheckBox(node.WorldPos, Vector3.one * nodeRadius * 0.9f, Quaternion.identity, SemiCoverMask))
            {
                List<A_Nodes> CoverinNode = GetNeighbours(node, false);
                foreach (A_Nodes coverinnode in CoverinNode)
                {
                    coverinnode.Nodetype = NodeType.SemiCover;
                }
            }
        }
    }
    /// <summary>
    /// �ֺ� ��� (8�溯 ��,��,��,�� �׸��� �밢�� 4����) ã�� �Լ� 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<A_Nodes> GetNeighbours(A_Nodes node,bool isDiagonal)
    {
        List<A_Nodes> neighbours = new List<A_Nodes>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;  //�ڱ� �ڽ��� ��� ��ŵ


                if (!isDiagonal && Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1) continue; //���� �Һ����� Ʈ�簡 �ƴϸ� �밢�� ����

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(this.node[checkX, checkY]);
                }
                
            }
        }
        return neighbours;
    }
    /// <summary>
    /// ����Ƽ �������������κ��� �׸������ ��带 ã�� �Լ�
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public A_Nodes GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2)/ gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2)/gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x =Mathf.RoundToInt((gridSizeX -1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY -1) * percentY);
        return node[x, y];
    }

    public void OnDrawGizmos()      //������������ ��������
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0, gridWorldSize.y));
        if (node != null)
        {
            foreach(A_Nodes node in node)
            {
                Gizmos.color = (node.Iswalkable) ? Color.white : Color.red;
                if (reachableNodes != null)
                {
                    if (reachableNodes.Contains(node))
                            Gizmos.color = Color.blue;
                }
                if (node.Nodetype == NodeType.Cover && node.Iswalkable || node.Nodetype == NodeType.SemiCover && node.Iswalkable)
                    Gizmos.color = Color.green;

                if (path != null)
                {
                    if (path.Contains(node))
                        Gizmos.color = Color.green;
                }
                Gizmos.DrawWireCube(node.WorldPos, Vector3.forward * (nodeDiameter - .1f) + Vector3.right * (nodeDiameter - .1f));
            }
        }
    }
}

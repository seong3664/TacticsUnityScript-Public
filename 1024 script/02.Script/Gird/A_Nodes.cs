using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Normal, Cover, SemiCover
}
public class A_Nodes
{
    public bool Iswalkable;
    public Vector3 WorldPos;
    public int gridX;
    public int gridY;

    public float gCost;
    public float hCost;
    public A_Nodes parentNode;

    private NodeType nodeType = NodeType.Normal;
    public NodeType Nodetype
    {
        get { return nodeType; }
        set { nodeType = value; }
    }
    public float fCost
    {
        get { return gCost + hCost; }
    }

    public A_Nodes(bool iswalkable, Vector3 worldPos,int nGridX,int nGridY)
    {
        Iswalkable = iswalkable;
        WorldPos = worldPos;
        gridX = nGridX;
        gridY = nGridY;
    }
   
}

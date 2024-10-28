using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Draw_movearea : MonoBehaviour
{

    LineRenderer Line;
    public List<A_Nodes> reachableNodes = new List<A_Nodes>();
    MoveCtrl MoveCtrl;

    public bool isDraw = false;
    Material quadMaterial;
    private Mesh quadMesh;
    A_Grid grid;
   [SerializeField]private Transform PointerUi;
    Image PointerUiImage;

    Transform highlihting;
    Transform Pointer;


    List<A_Nodes> neighbourNodes = new List<A_Nodes>();
    List<Vector3> boundaryarea = new List<Vector3>();
    List<Vector3> areaDrawdir = new List<Vector3>();
    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
        MoveCtrl = GetComponent<MoveCtrl>();
        PointerUi = transform.GetChild(0).GetChild(1).GetComponent<Transform>();
        PointerUiImage = PointerUi.GetComponent<Image>();
        grid = GameObject.Find("Grid_Manager").GetComponent<A_Grid>();
        highlihting = transform.GetChild(0).GetChild(2).GetComponent<Transform>();
        Pointer = transform.GetChild(0).GetChild(1).GetComponent<Transform>();

    }
    private void Start()
    {
        //quadMesh = CreateQuad(1f); // 쿼드 메쉬 생성
        Line.enabled = false;
        quadMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
    }
    /// <summary>
    /// 경로 따라서 라인 랜더러 그리는 매서드
    /// </summary>
    /// <param name="StartPos"></param>
    /// <param name="Nodepath"></param>
    public void Draw_MoveLine(Vector3 StartPos, List<A_Nodes> Nodepath)
    {
       
        if (Nodepath != null && Nodepath.Count >= 1)
        {
            PointerUiImage.enabled = true;
            highlihting.gameObject.SetActive(true);
            Line.enabled = true;
            Line.positionCount = Nodepath.Count + 1;
            Line.SetPosition(0, StartPos + Vector3.up * 0.05f); // 시작 위치 설정

            for (int i = 0; i < Nodepath.Count; i++)
            {
                Line.SetPosition(i + 1, Nodepath[i].WorldPos + Vector3.up * 0.05f); // 다음 위치 설정
            }
            PointerUi.position = Nodepath[Nodepath.Count-1].WorldPos+Vector3.up*0.01f;
            highlihting.position = Nodepath[Nodepath.Count - 1].WorldPos + Vector3.up * 0.01f;
            highlihting.rotation = highlihting.rotation;
        }
        else
        {
            Line.enabled = false;
            PointerUiImage.enabled = false;
            highlihting.gameObject.SetActive(false);
        }
    }
  
    public void boundarydraw()
    {
        boundaryarea.Clear();
        areaDrawdir.Clear();
        foreach (A_Nodes node in reachableNodes)
        {
            neighbourNodes = grid.GetNeighbours(node, false);
            foreach (A_Nodes neighbour in neighbourNodes)
            {
                if(!reachableNodes.Contains(neighbour) )
                {
                    boundaryarea.Add(node.WorldPos);
                    areaDrawdir.Add((node.WorldPos - neighbour.WorldPos));
                }
            }
        }
    }

    void OnRenderObject()
    {
        if (isDraw )
        {
            if (boundaryarea != null)
            {
                GL.PushMatrix();
                GL.LoadIdentity();
                GL.MultMatrix(Camera.main.worldToCameraMatrix);
                GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
                quadMaterial.SetPass(0); // 사용할 머티리얼 설정
                quadMaterial.color = Color.blue;
                GL.Begin(GL.LINES);
                

                for(int i = 0; i < areaDrawdir.Count; i++) 
                {
                    if (Mathf.Abs(areaDrawdir[i].x) > 0)
                    {
                        Vector3 start = boundaryarea[i] - areaDrawdir[i].normalized  + Vector3.up * 0.01f - Vector3.forward;
                        Vector3 end = boundaryarea[i] - areaDrawdir[i].normalized  + Vector3.up * 0.01f + Vector3.forward;
                        GL.Vertex(start);
                        GL.Vertex(end);
                    }
                    if (Mathf.Abs(areaDrawdir[i].z) > 0)
                    {
                        Vector3 start = boundaryarea[i] - areaDrawdir[i].normalized + Vector3.up * 0.01f - Vector3.right;
                        Vector3 end = boundaryarea[i] - areaDrawdir[i].normalized + Vector3.up * 0.01f + Vector3.right;
                        GL.Vertex(start);
                        GL.Vertex(end);
                    }

                }

                GL.End();
                GL.PopMatrix();
            }
        }
        else
        {
            Line.enabled = false;
            PointerUiImage.enabled = false;
            highlihting.gameObject.SetActive(false);
        }
    }

    



}


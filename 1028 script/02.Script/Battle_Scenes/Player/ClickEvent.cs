using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
public class ClickEvent : MonoBehaviour
{
    public A_Pathfinding A_Pathfinding;
    public A_Grid grid;
    public List<A_Nodes> reachableNodes = new List<A_Nodes>();
    List<A_Nodes> toMoveNodes = new List<A_Nodes>();
    Draw_movearea draw_Movearea;
    MoveCtrl moveCtrl;
    private Transform selectUnit;
    private UnitStat unitStat;



    bool canMove = true;
    LayerMask Player;
    LayerMask WalkAble;
    LayerMask Enemy;
    Unit_AniCtrl aniCtrl;

    private BtnEvent btnEvent;
    public AtkCamSet AtkCam;
    private GameObject GameManager;
    private Transform MoveUi;

    Quaternion UIrot;
    //Ŭ���ؼ� ����Ʈ ������ ������ ���� �ش� ���ֿ� �־�� ��ũ��Ʈ���� �ۿ��°� �ƴ϶�.
    //���� �Ŵ����� ����� ��ũ��Ʈ���� �־ �ش� ��ũ��Ʈ���� �� �ڵ��� ����Ʈ ������ ���������Ұ�.
    //�׷��� �� ���ָ��� ��ũ��Ʈ�� �־� ����� �ߺ��� ���� �� ���� ������? �ƹ�ư
    //�׸��ؼ� ���ֵ鿣 ��ũ��Ʈ�� �ּ������� �ְ�.(�� �������) ���� �Ŵ����� �־� ������� �ߺ��� ������.
    private void Awake()
    {
        moveCtrl = GetComponent<MoveCtrl>();
        draw_Movearea = GetComponent<Draw_movearea>();
        btnEvent = GameObject.Find("UI").GetComponent<BtnEvent>();
        GameManager = GameObject.Find("Game_Manager");
        MoveUi = transform.GetChild(0).GetComponent<Transform>();
    }
    private void Start()
    {
        turnManager.EndTurn += EndTurn;
        MoveUi.gameObject.SetActive(false);
        Player = 1 << 3;
        WalkAble = 1 << 7;
        Enemy = 1 << 8;
        UIrot = MoveUi.rotation;
    }
    private void Update()
    {

        if (turnManager.TurnManager.state != TurnState.PlayerTurn)
        {
            draw_Movearea.isDraw = false;
            MoveUi.gameObject.SetActive(false);
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if (!IsPointerOverUI())
        //{
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Player | WalkAble | Enemy) && canMove)
            {
                if (((1 << hit.collider.gameObject.layer) & WalkAble) != 0 && selectUnit != null && canMove && unitStat.stat.Action > 0)
                {
                    toMoveNodes = A_Pathfinding.A_StarAlgorithm(selectUnit.position, hit.point, unitStat.stat.MovePoint, false);
                    draw_Movearea.Draw_MoveLine(selectUnit.position, toMoveNodes);

                }
                // ���콺 ���� Ŭ�� ��
                if (Input.GetMouseButtonDown(0))
                {
                    // Player ���̾�� �浹�� ��� ���� ����
                    if (((1 << hit.collider.gameObject.layer) & Player) != 0)
                    {
                        selectUnit = hit.collider.transform;
                        btnEvent.BtnEventSetUnit(selectUnit);
                        MoveUi.SetParent(selectUnit);
                        MoveUi.position = selectUnit.position + Vector3.up * 0.01f;
                        MoveUi.localRotation = UIrot;
                        MoveUi.gameObject.SetActive(true);
                        unitStat = selectUnit.GetComponent<UnitStat>();

                       
                        aniCtrl = selectUnit.GetComponent<Unit_AniCtrl>();
                        if (unitStat.stat.Action > 0)
                        {
                            canMove = true;

                            StartCoroutine(DrawMoveArea());
                        }
                    }
                    // Walkable ���̾�� �浹�� ��� �̵� ����
                    else if (((1 << hit.collider.gameObject.layer) & WalkAble) != 0 && selectUnit != null && canMove && toMoveNodes != null && unitStat.stat.Action > 0)
                    {
                        canMove = false;
                        aniCtrl.MoveAniSet(true);
                        unitStat.stat.Action -= 1;
                        draw_Movearea.isDraw = false;
                        
                        StartCoroutine(moveCtrl.MoveNode(selectUnit, toMoveNodes));

                    }
                }
            }
        }
        //}
        if (!canMove && moveCtrl.IsMove)
        {
            aniCtrl.MoveAniSet(false);
           
            canMove = true;
            AtkCam.OffAtkCam(selectUnit);
            if (unitStat.stat.Action > 0)
            {
               StartCoroutine(DrawMoveArea());
            }
            else
            {
                btnEvent.BtnEventSetUnit(selectUnit);
            }
        }



    }
    void EndTurn()
    {
        selectUnit = null;
    }
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private IEnumerator DrawMoveArea()
    {

        reachableNodes = A_Pathfinding.GetReachableNodes(selectUnit.position, unitStat.stat.MovePoint);
        yield return new WaitForSeconds(0.3f);
        draw_Movearea.reachableNodes = reachableNodes;
        grid.reachableNodes = reachableNodes;

        draw_Movearea.boundarydraw();
        draw_Movearea.isDraw = true;
    }
}

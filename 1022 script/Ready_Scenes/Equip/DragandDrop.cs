using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragandDrop : MonoBehaviour
{
    private RectTransform DragItem;
    Equip DragItemEquip;
    private RectTransform SlotList;
    Transform BeginSlot;
    RectTransform DrupSlot;

    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    
    private Vector3 beginDragEquipPoint; 
    private Vector3 beginDragCursorPoint; 
    Vector3 beginDragPos;

    
    int unitnumber = 0;

    private void Awake()
    { 
        gr = GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        ped.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            DragItem = RaycastAndGetFirstComponent<Equip>();
            DragItemEquip = DragItem.GetComponent<Equip>();
            if (DragItem != null)
            {
                BeginSlot = DragItem.parent;
                beginDragEquipPoint = DragItem.position;
                beginDragCursorPoint = Input.mousePosition;
                DragItem.SetParent(transform);
            }
        }
        if (Input.GetMouseButton(0) && DragItem != null)
        {
            DragItem.position =
           beginDragEquipPoint + (Input.mousePosition - beginDragCursorPoint);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (DragItem != null)
            {
                DrupSlot = RaycastAndGetFirstComponent<Unit_Equip_Slot>();

                if (DrupSlot != null && DrupSlot.childCount == 0)
                {
                    Debug.Log(DrupSlot.name);
                    DragItem.SetParent(DrupSlot);
                }
                else
                {
                    DragItem.SetParent(BeginSlot);
                    DragItem.position = beginDragCursorPoint;
                }
            }

        }
    }
    private RectTransform RaycastAndGetFirstComponent<T>() where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);
        Debug.Log(rrList.Count);
        if (rrList.Count == 0)
            return null;


        return rrList[0].gameObject.GetComponent<RectTransform>();
    }
}

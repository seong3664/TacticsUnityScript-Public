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
    Unit_Equip_Slot DrupSlotEquip;
    RectTransform DragSlot;
    Unit_Equip_Slot DragSlotEquip;
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    
    private Vector3 beginDragEquipPoint; 
    private Vector3 beginDragCursorPoint; 
    Vector3 beginDragPos;

    
    

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
            SoundManager.instance.PlayerSound("ClickSound");
            DragItemEquip = RaycastAndGetFirstComponent<Equip>();

            if (DragItemEquip != null)
            {
                SoundManager.instance.PlayerSound("BtnSound");
                //Debug.Log(DragItemEquip.name);
                DragItem = DragItemEquip.gameObject.GetComponent<RectTransform>();
            }

            DragSlotEquip = RaycastAndGetFirstComponent<Unit_Equip_Slot>();
            if (DragSlotEquip != null)
                DragSlot = DragSlotEquip.gameObject.GetComponent<RectTransform>();   
            
            if (DragItem != null)
            {
                SoundManager.instance.PlayerSound("BtnSound");
                BeginSlot = DragItem.parent;
                beginDragEquipPoint = DragItem.position;
                beginDragCursorPoint = Input.mousePosition;
                DragItem.SetParent(transform);
                DragSlotEquip.OutItem(DragItemEquip.equip_Inpo);
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
                DrupSlotEquip = RaycastAndGetFirstComponent<Unit_Equip_Slot>();
                if (DrupSlotEquip != null)
                    DrupSlot = DrupSlotEquip.gameObject.GetComponent<RectTransform>();
                if (DrupSlot != null && DrupSlot.childCount == 0)
                {

                    DragItem.SetParent(DrupSlot);
                    if (DragItem.parent != BeginSlot)
                    DrupSlotEquip.AddItem(DragItemEquip.equip_Inpo);
                }
                else
                {
                    DragItem.SetParent(BeginSlot);
                    DragItem.position = beginDragCursorPoint;
                }
                DragItem = null;
                DragItemEquip = null;
            }

        }
    }
    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);
        
        foreach (var result in rrList)
        {
            T component = result.gameObject.GetComponent<T>();
            if (component != null)
                return component;
        }

            return null;

    }
}

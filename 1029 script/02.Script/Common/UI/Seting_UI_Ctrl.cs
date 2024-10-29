using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seting_UI_Ctrl : MonoBehaviour
{
    GameObject SetingMenu;
    Button SetingMenuOnoffbtn;
    [SerializeField]
    GameObject OtherUI;

    private void Awake()
    {
        SetingMenu = transform.GetChild(0).gameObject;
        SetingMenuOnoffbtn = transform.GetChild(1).GetComponent<Button>();
    }
    private void Start()
    {
        SetingMenuOnoffbtn.onClick.RemoveAllListeners();
        SetingMenuOnoffbtn.onClick.AddListener(SetingMenuOnoff);
    }
    void SetingMenuOnoff()
    {
        OtherUI.SetActive(SetingMenu.activeSelf);
        SetingMenu.SetActive(!SetingMenu.activeSelf);       
    }
}

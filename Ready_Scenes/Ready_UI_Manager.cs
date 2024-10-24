using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
public class Ready_UI_Manager : MonoBehaviour
{
    Transform Ready_UI_Canvas;
    GameObject Unit_Editor;
    Button Unit_Editorbtr;

    private void Awake()
    {
        Ready_UI_Canvas = GameObject.Find("Ready_UI").GetComponent<Transform>();
        Unit_Editorbtr = Ready_UI_Canvas.GetChild(1).GetComponent<Button>();
        Unit_Editor = Ready_UI_Canvas.GetChild(2).gameObject;
    }
    private void Start()
    {
        Unit_Editorbtr.onClick.RemoveAllListeners();
        Unit_Editorbtr.onClick.AddListener(() => Unit_Editor.SetActive(!Unit_Editor.activeSelf));
        Unit_Editor.SetActive(false);
    }
}

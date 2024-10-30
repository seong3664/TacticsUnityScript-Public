using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetingMenuQuitBtn : MonoBehaviour
{
    List<Button> quitBoxOnOffBtn = new List<Button>();
    Button quitbtn;
    GameObject quitcheckBox;
    Transform SetingMenuBtn;
    private void Awake()
    {
        SetingMenuBtn = transform.parent.GetChild(1);  //버튼들 기능이 달려있는 패널 담아놓은 오브젝트
        quitcheckBox = transform.GetChild(3).gameObject;   //종료 버튼시 진짜 종료할지 아닐지 확인하는 창
        quitBoxOnOffBtn.Add(SetingMenuBtn.GetChild(3).GetComponent<Button>());  //quit버튼 누를시 확인창 on 열려있을 때 누르면 off
        quitBoxOnOffBtn.Add(quitcheckBox.transform.GetChild(0).GetChild(2).GetComponent<Button>()); //quit 버튼 누르면 나오는 확인창에서 no버튼
        quitbtn = quitcheckBox.transform.GetChild(0).GetChild(1).GetComponent<Button>();



    }
    private void Start()
    {
        foreach (var button in quitBoxOnOffBtn)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(QuitBtnOnOff);
        }
        quitbtn.onClick.RemoveAllListeners();
        quitbtn.onClick.AddListener(QuitBtn);
    }

    public void QuitBtnOnOff()
    {
        quitcheckBox.SetActive(!quitcheckBox.activeSelf);
    }
    public void QuitBtn()
    {
#if UNITY_EDITOR    
        UnityEditor.EditorApplication.isPlaying = false;  
#else 
     Application.Quit();
#endif
    }
}

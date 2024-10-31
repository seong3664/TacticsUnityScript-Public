using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Seting_UI_Ctrl : MonoBehaviour
{
    GameObject SetingMenu;
    Button SetingMenuOnoffbtn;
    [SerializeField]
    GameObject[] OtherUI;
     
    GameObject SaveLoad_Set;
    GameObject Video_Set;
    GameObject Sound_Set;
    private void Awake()
    {
        SetingMenu = transform.GetChild(0).gameObject;
        SetingMenuOnoffbtn = transform.GetChild(1).GetComponent<Button>();
        Video_Set = transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        SaveLoad_Set = transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        Sound_Set = transform.GetChild(0).GetChild(2).GetChild(2).gameObject;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        SceneManager.sceneLoaded += LoadUInewScene;
        LoadUI();
        SetingMenuOnoffbtn.onClick.RemoveAllListeners();
        SetingMenuOnoffbtn.onClick.AddListener(SetingMenuOnoff);
    }
    public void SetingMenuOnoff()
    {
        for (int i = 0; i < OtherUI.Length; i++)
        {
            if(OtherUI[i] != null)
                 OtherUI[i].SetActive(SetingMenu.activeSelf);
        }
        SetingMenu.SetActive(!SetingMenu.activeSelf);       
    }
    
    private void LoadUInewScene(Scene scene, LoadSceneMode mode)
    {
        LoadUI();
    }
    void LoadUI()
    {
        Canvas[] UI = FindObjectsOfType<Canvas>();
        OtherUI = new GameObject[UI.Length];
        for (int i = 0; i < UI.Length; i++)
        {
            if (UI[i].gameObject != this.gameObject)
                OtherUI[i] = UI[i].gameObject;
        }
    }
    public void VideoMenuOn()
    {
        Video_Set.SetActive(true);
        Sound_Set.SetActive(false);
        SaveLoad_Set.SetActive(false);
    }
    public void SoundMeniOn()
    {
        Video_Set.SetActive(false);
        Sound_Set.SetActive(true);
        SaveLoad_Set.SetActive(false);
    }
    public void SaveMenuOn()
    {
        Video_Set.SetActive(false);
        Sound_Set.SetActive(false);
        SaveLoad_Set.SetActive(true);
    }
}

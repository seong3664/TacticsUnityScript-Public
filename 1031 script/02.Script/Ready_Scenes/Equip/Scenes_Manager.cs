
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Scenes_Manager : MonoBehaviour
{
    Canvas ReadyScenesUI;
    Button StartButton;
    private void Awake()
    {
        ReadyScenesUI = GameObject.Find("Ready_UI").GetComponent<Canvas>();
        StartButton = ReadyScenesUI.transform.GetChild(0).GetComponent<Button>();
    }
    private void Start()
    {
        StartButton.onClick.RemoveAllListeners();
        StartButton.onClick.AddListener(LoadBattleScenes);
    }
    void LoadBattleScenes()
    {
        GameManager.gamemaneger.isnewGame = true;
        GameManager.gamemaneger.Stats = UnitSet_Manager.instance.unitstat;
        SceneManager.LoadScene("Battle_Scenes");
    }    
}

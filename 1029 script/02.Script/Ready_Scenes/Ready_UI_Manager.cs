
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class Ready_UI_Manager : MonoBehaviour
{
    GameObject Unit_Editor;
    Button Unit_Editorbtr;

    TMP_Text UnitstatText;
    UnitStat Unitstat;
    private void Awake()
    {
        Unit_Editorbtr = transform.GetChild(1).GetComponent<Button>();
        Unit_Editor = transform.GetChild(2).gameObject;
        UnitstatText = transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }
    private void Start()
    {
        Unit_Editorbtr.onClick.RemoveAllListeners();
        Unit_Editorbtr.onClick.AddListener(() => Unit_Editor.SetActive(!Unit_Editor.activeSelf));
        Unit_Editor.SetActive(false);
        Unitstat = UnitSet_Manager.instance.Playerbase;
    }
    private void FixedUpdate()
    {
        if (Unitstat != UnitSet_Manager.instance.Playerbase)
        {
            Unitstat = UnitSet_Manager.instance.Playerbase;
        }
        UnitstatText.text = $"Hp: {Unitstat.stat.Hp}\n\n" +
            $"MovePoint: {Unitstat.stat.MovePoint}\n\n" +
            $"Aiming: {Unitstat.stat.Aiming}\n\n" +
            $"Evasion: {Unitstat.stat.Evasion}\n\n" +
            $"Crit: {Unitstat.stat.Crit}";
                
    }       
}

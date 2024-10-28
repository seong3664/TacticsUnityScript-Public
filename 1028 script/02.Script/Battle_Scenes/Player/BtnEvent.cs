using States;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnEvent : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject BackGround;
    [SerializeField] Transform SelectUnit;
    [SerializeField] Transform ATK_Target;
    [SerializeField] Button[] Target_Change_Btns;

    AtkCamSet AtkCam;
    Unit_AniCtrl AniCtrl;
    Unit_Fire FireCtrl;
    public TMP_Text[] backGroundText;
    UnitStat UnitStat;
    List<Transform> CanAtkE = new List<Transform>();

    Unit_crouch unitcrouch;
    AimCheck aimCheck;
    private void Awake()
    {
        BackGround = transform.GetChild(0).gameObject;
        buttons = transform.GetChild(1).GetComponentsInChildren<Button>();
        backGroundText = BackGround.transform.GetChild(0).GetComponentsInChildren<TMP_Text>();
        foreach (var button in buttons)
             button.gameObject.SetActive(false);
        Target_Change_Btns = transform.GetChild(3).GetComponentsInChildren<Button>();
        aimCheck = GameObject.Find("Enemy_Manager").GetComponent<AimCheck>();
        unitcrouch = GetComponent<Unit_crouch>();
    }
    private void Start()
    {
        //for (int i = 1; i < Target_Change_Btns.Length; i++)
        //{
        //    int Index = i;
        //    Debug.Log(Index);
        //    Target_Change_Btns[i].GetComponent<Button>().onClick.AddListener(() => TargetChageBtn(Index));
        //    Debug.Log(Target_Change_Btns[i].GetComponent<Button>());
        //}
        foreach (var Target_Change_Btn in Target_Change_Btns)
        {
            Target_Change_Btn.gameObject.SetActive(false);
        }
        AtkCam = GetComponent<AtkCamSet>();
        
        BackGround.SetActive(false);
    }   
    /// <summary>
    /// 버튼 onoff 함수
    /// </summary>
    /// <param name="Unit"></param>
    public void BtnEventSetUnit(Transform Unit)
    {
       UnitStat = Unit.GetComponent<UnitStat>();
        if (Unit != null && UnitStat.stat.Action > 0)
        {
            SelectUnit = Unit;
            AniCtrl = Unit.GetComponent<Unit_AniCtrl>();
            FireCtrl = Unit.GetComponent<Unit_Fire>();
            foreach (var button in buttons)
                button.gameObject.SetActive(true);
            if(UnitStat.BulletCount <= 0)
            {
                buttons[0].gameObject.SetActive(false);
            }
        }
        
        else
        {
            for(int i = 0; i < buttons.Length-1; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            buttons[buttons.Length-1].gameObject.SetActive(true);
        }
    }
    public void Reload()
    {
        AniCtrl.ReloadAniSet();
        UnitStat.BulletCount = 3;
        UnitStat.stat.Action -= 1;
        BtnEventSetUnit(SelectUnit);
    }
    public void CrouchUnit()
    {
        unitcrouch.CoruchUnit(SelectUnit);
        AniCtrl.CoverKneeSet(true);
        BtnEventSetUnit(SelectUnit);
    }
    public void ShoutBtn()
    {
        ChangeTarget();
    }

    private void ChangeTarget()
    {
        CanAtkE.Clear();
        RaycastHit hit;
        Collider[] inrange = Physics.OverlapSphere(SelectUnit.position, 30f, 1 << 8);

        for (int i = 0; i < inrange.Length; i++)
        {
            if (Physics.Raycast(new Ray(SelectUnit.position + Vector3.up, (inrange[i].transform.position - SelectUnit.position).normalized), out hit, 30f, 1 << 8))
            {
                Debug.DrawRay(SelectUnit.position + Vector3.up, (inrange[i].transform.position - SelectUnit.position).normalized * 30f, Color.blue, 50f);
                CanAtkE.Add(inrange[i].transform);
                Target_Change_Btns[i].gameObject.SetActive(true);
                int index = i;
                Target_Change_Btns[i].onClick.RemoveAllListeners();
                Target_Change_Btns[i].onClick.AddListener(() => TargetChageBtn(index));
            }

        }

        if (CanAtkE.Count > 0)
        {
            turnManager.TurnManager.state = TurnState.WaitingTurn;
            ATK_Target = CanAtkE[0];
            int HitChance = aimCheck.HitChanceCheckTransform(SelectUnit, CanAtkE[0]);
            BackGround.SetActive(true);
            backGroundText[0].text = $"Hit:{HitChance.ToString()}%";
            backGroundText[1].text = $"{UnitStat.stat.dmg.ToString()}";
            AniCtrl.AimAniSet(true);

            AtkCam.SetSelectUnitCam(SelectUnit, ATK_Target);
        }
    }

    public void TargetChageBtn(int index)
    {
        ATK_Target = CanAtkE[index].transform;
        AtkCam.SetSelectUnitCam(SelectUnit,CanAtkE[index]);
        int HitChance = aimCheck.HitChanceCheckTransform(SelectUnit, ATK_Target);
        backGroundText[0].text = $"Hit:{HitChance.ToString()}%";
    }
    public void FireBtn()
    {
        StartCoroutine(ATKTime());
        FireCtrl.FireRay(ATK_Target,UnitStat.stat.dmg);
        for (int i = 0; i < Target_Change_Btns.Length; i++)
        {
            Target_Change_Btns[i].gameObject.SetActive(false);
        }
        StartCoroutine(HideButtn());
        
    }
    public void EndTurn()
    {
        if (AtkCam.IsAtkCamOn)
        {
            BackGround.SetActive(false);
            AtkCam.OffAtkCam(SelectUnit);
            BtnEventSetUnit(SelectUnit);
            turnManager.TurnManager.state = TurnState.PlayerTurn;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < Target_Change_Btns.Length; i++)
            {
                Target_Change_Btns[i].gameObject.SetActive(false);
            }
            AniCtrl.AimAniSet(false);
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            turnManager.TurnManager.EndPlayerTunr();
        }       
    }
    IEnumerator ATKTime()
    {

        AniCtrl.AtkAniSet(true);
        yield return new WaitForSeconds(0.5f);
        AniCtrl.AtkAniSet(false);
    }
    IEnumerator HideButtn()
    {
        BackGround.SetActive(false);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1);
        AtkCam.OffAtkCam(SelectUnit);
        AniCtrl.AimAniSet(false);
        turnManager.TurnManager.state = TurnState.PlayerTurn;
        BtnEventSetUnit(SelectUnit);
    }
}

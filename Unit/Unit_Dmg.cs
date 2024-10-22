using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Dmg : MonoBehaviour, OnDamege
{
    UnitStat Unitstat;
    Unit_AniCtrl Unit_AniCtrl;
    CapsuleCollider Collider;
    Rigidbody[] Rigidbody;
    Animator animator;

    GameObject MissTxt;
    void Awake()
    {
        Unitstat = GetComponent<UnitStat>();
        Unit_AniCtrl = GetComponent<Unit_AniCtrl>();
        Collider = GetComponent<CapsuleCollider>();
        Rigidbody = transform.GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        MissTxt = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
    }
    private void Start()
    {
        MissTxt.SetActive(false);
    }
    public void OnDamege(int Damege, Vector3 hitPoint, Vector3 hitnom)
    {
        Debug.Log("¿€µø«‘?");
        Unitstat.stat.Hp -= Damege;
        
        if (Unitstat.stat.Hp <= 0)
        {
            animator.enabled = false;
            UnitDie(hitPoint,hitnom);
        }
        else
        {
            Unit_AniCtrl.HitAniSet();
        }
    }
    public void OnMissed()
    {
        StartCoroutine(MissTxtOn());
    }
    IEnumerator MissTxtOn()
    {
        MissTxt.SetActive (true);
        yield return new WaitForSeconds(1.5f);
        MissTxt.SetActive(false);
    }
    void UnitDie(Vector3 hitPoint, Vector3 hitnom)
    {
        
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.gameObject.tag = "Untagged";
        Collider.enabled = false;
        Rigidbody[0].isKinematic = true;
        Vector3 horizontalHitnom = new Vector3(hitnom.x, 0, hitnom.z);
       for (int i = 0; i < Rigidbody.Length  ; i++) 
        {
             if (i != 0)
            {
                Rigidbody[i].isKinematic = false;
                 Rigidbody[i].AddForce(-horizontalHitnom, ForceMode.Impulse);
            }
           
        }
    }

    

}

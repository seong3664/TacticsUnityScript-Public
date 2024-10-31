using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Dmg : MonoBehaviour, OnDamege
{
    UnitStat Unitstat;
    Unit_AniCtrl Unit_AniCtrl;
    CapsuleCollider Collider;
    Rigidbody Rigidbody;
    Animator animator;

    GameObject MissTxt;
    ParticleSystem Bloodeff;
    void Awake()
    {
        Unitstat = GetComponent<UnitStat>();
        Unit_AniCtrl = GetComponent<Unit_AniCtrl>();
        Collider = GetComponent<CapsuleCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        MissTxt = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        Bloodeff = transform.GetChild(2).GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        MissTxt.SetActive(false);
    }
    public void OnDamege(int Damege, Vector3 hitPoint, Vector3 hitnom)
    {
        Unitstat.stat.Hp -= Damege;
        Bloodeff.transform.position = hitPoint;
        Bloodeff.transform.rotation = Quaternion.LookRotation(hitnom);
        Bloodeff.Play();
        SoundManager.instance.PlayerSound("Hit");
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
        if (Unitstat.stat.Hp <= 0)
        {
            animator.enabled = false;
        }
        else
        {
            Unit_AniCtrl.HitAniSet();
        }
    }
    IEnumerator MissTxtOn()
    {
        SoundManager.instance.PlayerSound("Miss");
        MissTxt.SetActive (true);
        yield return new WaitForSeconds(2f);
        MissTxt.SetActive(false);
    }
    void UnitDie(Vector3 hitPoint, Vector3 hitnom)
    {
        
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.gameObject.tag = "Untagged";
        Collider.enabled = false;
        Rigidbody.isKinematic = false;
        Vector3 horizontalHitnom = new Vector3(hitnom.x, 0, hitnom.z);
        Rigidbody.AddForce(horizontalHitnom);
        
    }

    
}

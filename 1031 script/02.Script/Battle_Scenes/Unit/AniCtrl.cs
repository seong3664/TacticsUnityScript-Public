using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AniCtrl : MonoBehaviour
{
    Animator animator;
    public Transform gunPivot;
    public Transform leftHandMount; // √—¿« øﬁ¬  º’¿‚¿Ã, øﬁº’¿Ã ¿ßƒ°«“ ¡ˆ¡°
    public Transform rightHandMount;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void MoveAniSet(bool isMove)
    {
        animator.SetBool("Move", isMove);
    }
    public void AimAniSet(bool Aiming)
    {
        animator.SetBool("Aiming",Aiming);
    }
    public void AtkAniSet(bool Atk)
    {
        animator.SetBool("ATK",Atk);
    }
    public void HitAniSet()
    {
        animator.SetTrigger("Hit");
    }
    public void DieAniSet()
    {
        animator.SetTrigger("Die");
    }
    public void CoverKneeSet(bool Cover)
    {
        animator.SetBool("Cover",Cover);
    }
    public void ReloadAniSet()
    {
        animator.SetTrigger("Reload");
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //bool isRunning = animator.GetCurrentAnimatorStateInfo(0).IsName("Move");

        //if (isRunning)
        //{
        //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        //}
        //else
        //{
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);


        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand,
            leftHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand,
            leftHandMount.rotation);


        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand,
            rightHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand,
            rightHandMount.rotation);
        //}
    }

}

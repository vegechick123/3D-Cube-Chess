using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class CAnimationMoveComponent : CMoveComponent
{
    [HideInInspector]
    public Animator animator;
    private GameObject animObject;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animObject = transform.GetChild(0).gameObject;
    }
    protected override void Update()
    {
        //base.Update();
    }
    public override bool RequestMove(Vector2Int[] pathArr)
    {
        bool res = base.RequestMove(pathArr);
        if (res)
        {
            animator.Play("Move");
            Vector3 dir = curTargetPosition - transform.position;
            if(dir.magnitude>0.01)
                transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
        }
        return res;
    }
    public void OneMoveComplete()
    {
        transform.position = curTargetPosition;
        if(!NextPosition())
        {
            animator.Play("Idle");
        }
        else
        {
            Vector3 dir = curTargetPosition - transform.position;
            transform.rotation=Quaternion.LookRotation(dir.normalized,Vector3.up);
            animator.Play("Move");
        }
    }
}

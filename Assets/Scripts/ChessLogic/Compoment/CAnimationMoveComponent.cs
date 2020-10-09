using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;

public class CAnimationMoveComponent : CMoveComponent
{
    [HideInInspector]
    public Animator animator;
    private GameObject animObject;
    [NonSerialized]
    protected bool useAnimation = true;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animObject = transform.GetChild(0).gameObject;
    }
    protected override void Update()
    {
        if(!useAnimation)
            base.Update();
    }
    public override bool RequestMove(Vector2Int[] pathArr)
    {
        bool res = base.RequestMove(pathArr);
        if (res)
        {
            animator.Play("Move");
            Vector3 dir = curTargetPosition - transform.position;
            if (dir.magnitude > 0.01)
                (actor as GChess).FaceToward(dir);
        }
        return res;
    }
    public override bool RequestDirectMove(Vector2Int destination)
    {
        useAnimation = false;
        eFinishPath.AddListener(() => useAnimation = true);
        UnityAction t = null;
        t = () =>
        {
            useAnimation = true;
            eFinishPath.RemoveListener(t);
        };
        
        eFinishPath.RemoveListener(t);
        return base.RequestMove(new Vector2Int[] {destination});
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

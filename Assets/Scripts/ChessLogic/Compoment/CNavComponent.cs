using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(CMoveComponent), typeof(GChess))]
public class CNavComponent : Component
{
    [HideInInspector]
    public NavInfo navInfo;
    [HideInInspector]
    public CMoveComponent moveComponent;

    [HideInInspector]
    public CAgentComponent agentComponent;
    protected override void Awake()
    {
        base.Awake();
        moveComponent = GetComponent<CMoveComponent>();
        agentComponent = GetComponent<CAgentComponent>();
    }
    public void GenNavInfo()
    {
        navInfo = GridManager.instance.GetNavInfo(location,curMovement);
    }
    public void MoveToWtihNavInfo(Vector2Int destination)
    {
        Vector2Int[] path= navInfo.GetPath(destination);
        moveComponent.RequestMove(path);
    }
}

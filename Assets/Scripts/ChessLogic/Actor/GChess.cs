using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GChess : GActor
{
    public int health;
    [HideInInspector]
    public int curHealth;

    public int movement;
    [HideInInspector]
    public int curMovement;

    public int teamID;

    [HideInInspector]
    public List<Skill> skills;

    protected CAgentComponent agentComponent;
    protected CNavComponent navComponent;
    protected CMoveComponent moveComponent;

    protected override void Awake()
    {
        base.Awake();
        GameManager.instance.eTurnStart[teamID].AddListener(OnTurnStart);
        GameManager.instance.eTurnEnd[teamID].AddListener(OnTurnEnd);
        curHealth = health;
        curMovement = movement;
        agentComponent = GetComponent<CAgentComponent>();
        if(agentComponent)
        {
            agentComponent.eSelect.AddListener(OnSelect);
            agentComponent.eDeselect.AddListener(OnDeselect);
        }
        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();

        skills=new List<Skill>(GetComponents<Skill>());
    }
    protected virtual void OnSelect()
    {

    }
    protected virtual void OnDeselect()
    {

    }
    virtual protected void OnTurnStart()
    {

    }
    virtual protected void OnTurnEnd()
    {

    }
    public void MoveTo(GFloor floor)
    {
        MoveTo(floor.location);
    }
    public void MoveTo(Vector2Int _location)
    {
        location = _location;
        if(navComponent)
        {
            navComponent.MoveToWtihNavInfo(_location);
        }
        else if(moveComponent)
        {
            Debug.LogError("Move To without navComponent");
        }
        else
        {
            Debug.LogError("Move To without moveComponent");
        }
    }
}

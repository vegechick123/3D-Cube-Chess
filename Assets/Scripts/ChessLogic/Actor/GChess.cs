using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GChess : GActor
{
    public int health=3;
    [HideInInspector]
    public int curHealth;

    public int movement=3;
    [HideInInspector]
    public int curMovement;

    public int teamID;

    public List<Skill> skills;

    protected UnityEvent eLocationChange= new UnityEvent();
    protected UnityEvent eMovementChange = new UnityEvent();
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
        GridManager.instance.AddChess(this);
        foreach(var skill in skills)
        {
            skill.owner = this;
        }
    }
    protected virtual void OnSelect()
    {
        if(curMovement>0)
        {
            navComponent.GenNavInfo();
            MoveCommand moveCommand = new MoveCommand(navComponent.navInfo.range, this, MoveTo);
            moveCommand.CreateFloorHUD(new Color(0, 1, 0, 0.5f));
            PlayerControlManager.instance.GenMoveCommand(moveCommand);
        }
        ShowUI();
    }
    protected virtual void OnDeselect()
    {
        HideUI();
    }
    virtual protected void OnTurnStart()
    {
        curMovement = movement;
    }
    virtual protected void OnTurnEnd()
    {

    }
    protected void ShowUI()
    {
        UIManager.instance.SwitchSkillButton(skills.ToArray());
    }
    protected void HideUI()
    {
        UIManager.instance.CleanSkillButton();
    }
    #region 位移相关
    /// <summary>
    /// 向一个方向推动这个Chess,如果遇到障碍物则停下
    /// </summary>
    /// <param name="direction">方向，请保证是单位向量</param>
    /// <param name="distance">推动的距离</param>
    public void PushToward(Vector2Int direction,int distance)
    {
        Vector2Int destination = location;
        for(int i=0;i<distance;i++)
        {
            if (GridManager.instance.GetChess(destination + direction))
            {
                break;
            }
            else
            {
                destination = destination + direction;
            }

        }
        MoveToDirectly(destination);
    }
    /// <summary>
    /// 不通过寻路径直走向终点
    /// </summary>
    /// <param name="destination">终点</param>
    public void MoveToDirectly(Vector2Int destination)
    {
        location = destination;
        moveComponent.RequestMove(new Vector2Int[] { destination});
    }
    public void MoveTo(GFloor floor)
    {
        MoveTo(floor.location);
    }
    public void MoveTo(Vector2Int destination)
    {
        location = destination;
        if(navComponent)
        {
            
            navComponent.MoveToWtihNavInfo(destination);
            curMovement = 0;
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
    public void Teleport(Vector2Int destination)
    {
        location = destination;
        transform.position = GridManager.instance.GetChessPosition3D(location);
    }
    #endregion
}

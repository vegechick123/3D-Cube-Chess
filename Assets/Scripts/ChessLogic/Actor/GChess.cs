using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class GChess : GActor
{
    public int health=3;
    [HideInInspector]
    public int curHealth { get; protected set; }

    public int movement=3;
    [HideInInspector]
    public int curMovement;

    public int teamID;

    protected UnityEvent eLocationChange= new UnityEvent();
    protected UnityEvent eMovementChange = new UnityEvent();
    [HideInInspector]
    public CNavComponent navComponent;
    [HideInInspector]
    public CMoveComponent moveComponent;

    protected override void Awake()
    {
        base.Awake();
        
        GameManager.instance.eRoundStart.AddListener(OnRoundStart);
        GameManager.instance.eRoundEnd.AddListener(OnRoundEnd);
        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();
        GridManager.instance.AddChess(this);
        
    }
    protected override void OnGameStart()
    {
        base.OnGameStart();
        OnBirth();
    }
    protected virtual void OnBirth()
    {
        curHealth = health;
        curMovement = movement;
    }
    
    override protected void OnRoundStart()
    { 
        curMovement = movement;
    }
    public void Recover(int value)
    {
        curHealth += value;
        curHealth = Mathf.Min(curHealth, health);
    }
    public void Damage(int value)
    {
        curHealth -= value;
        if(curHealth<=0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(this);
    }
    protected virtual void OnDestroy()
    {
        GridManager.instance.RemoveChess(this);

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
            Vector2Int curLocation = destination + direction;
            if (GridManager.instance.GetChess(curLocation)
                ||!GridManager.instance.InRange(curLocation))
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
        navComponent.GenNavInfo();
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

using UnityEngine;
using UnityEngine.Events;

public class GChess : GActor
{
    //无法行动
    [HideInInspector]
    public bool unableAct { get; protected set; }
    public bool hasActed=false;
public int health = 3;
    //[HideInInspector]
    public int curHealth { get; protected set; }

    public int movement = 3;
    [HideInInspector]
    public int curMovement;

    public int teamID;

    protected UnityEvent eLocationChange = new UnityEvent();
    protected UnityEvent eMovementChange = new UnityEvent();
    [HideInInspector]
    public CNavComponent navComponent;
    [HideInInspector]
    public CMoveComponent moveComponent;

    public HealthBar healthBar;
    public bool immuniateEnvironmentColdness;
    public bool melt;

    protected override void Awake()
    {
        base.Awake();

        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();
        GridManager.instance.AddChess(this);
        healthBar = new HealthBar(this);
    }
    public override void OnGameStart()
    {
        base.OnGameStart();
        OnBirth();
    }
    protected virtual void OnBirth()
    {
        curHealth = health;
        curMovement = movement;
        healthBar.Refresh();
    }

    override protected void OnRoundStart()
    {
        curMovement = movement;
        hasActed = false;
    }
    protected override void OnRoundEnd()
    {
        base.OnRoundEnd();
        int t=TempertureManager.instance.GetTempatureAt(location);
        if(t<0)
        {
            if(!immuniateEnvironmentColdness)
                ElementReaction(Element.Ice);
        }  
        else if(t>0)
        {
            ElementReaction(Element.Fire);
        }
    }
    public void Recover(int value)
    {
        curHealth += value;
        curHealth = Mathf.Min(curHealth, health);
    }
    public void ElementDamage(Element element, int damage)
    {
        if (elementComponent)
        {
            //damage = elementComponent.ProcessDamage(element, damage);
            Damage(damage);
            elementComponent.OnHitElement(element);
        }
        else
        {
            Damage(damage);
        }
    }
    public void Damage(int value)
    {
        curHealth -= value;
        if (curHealth <= 0)
        {
            DieImmediately();
        }
        healthBar.Refresh();
    }
    public void DieImmediately()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }
    protected virtual void OnDestroy()
    {
        if(GridManager.instance)
            GridManager.instance.RemoveChess(this);

    }
    #region 位移相关
    /// <summary>
    /// 向一个方向推动这个Chess,如果遇到障碍物则停下
    /// </summary>
    /// <param name="direction">方向，请保证是单位向量</param>
    /// <param name="distance">推动的距离</param>
    public void PushToward(Vector2Int direction, int distance)
    {
        Vector2Int destination = location;
        for (int i = 0; i < distance; i++)
        {
            Vector2Int curLocation = destination + direction;
            if (GridManager.instance.GetChess(curLocation)
                || !GridManager.instance.InRange(curLocation))
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
        Debug.Log("MoveToDirectly " + destination);
        moveComponent.RequestDirectMove(destination);
        location = destination;
        moveComponent.eFinishPath.AddListener(EnterLocation);
    }
    public void MoveTo(GFloor floor)
    {
        MoveTo(floor.location);
    }
    public void MoveTo(Vector2Int destination)
    {
        Debug.Log("MoveTo " + destination);
        navComponent.GenNavInfo();
        if (navComponent)
        {
            moveComponent.eFinishPath.AddListener(EnterLocation);
            navComponent.MoveToWtihNavInfo(destination);
            location = destination;
            curMovement = 0;
        }
        else if (moveComponent)
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
        GridManager.instance.GetFloor(location).OnChessEnter(this);
    }
    public void EnterLocation()
    {
        GridManager.instance.GetFloor(location).OnChessEnter(this);
        moveComponent.eFinishPath.RemoveListener(EnterLocation);
    }
    #endregion
    public virtual void DisableAction()
    {
        unableAct = true;
    }
    public virtual void ActiveAction()
    {
        unableAct = false;
    }
    public override void ElementReaction(Element element)
    {
        base.ElementReaction(element);
        if (melt&&element == Element.Fire)
            Damage(1);
    }
    public void FaceToward(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }
    public void FaceToward(Vector2Int dir)
    {
        FaceToward(new Vector3(dir.x,0,dir.y));
    }

}

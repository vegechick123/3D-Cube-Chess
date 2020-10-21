using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GChess : GActor
{
    //无法行动
    [HideInInspector]
    public bool freezeFoot=false;
    [HideInInspector]
    public UnityEvent eFreezeFootBroken = new UnityEvent();
    protected GameObject freezenFootVFX;


    public GameObject prefabFreezenFootVFX;

    public GameObject prefabFreezenFootBrokenVFX;
    protected bool warm;

    protected GameObject warmVFX;
    public GameObject prefabWarmVFX;
    public bool unableAct { get; protected set; }
    public bool hasActed=false;
    public int health = 3;
    //[HideInInspector]
    public int curHealth { get; protected set; }

    public int movement = 3;
    [HideInInspector]
    public int curMovement;

    public int teamID;

    public UnityEvent eLocationChange = new UnityEvent();
    public UnityEvent eBeForceMove = new UnityEvent();
    protected UnityEvent eMovementChange = new UnityEvent();
    [HideInInspector]
    public CNavComponent navComponent;
    [HideInInspector]
    public CMoveComponent moveComponent;
    [HideInInspector]
    public Outline outline;

    public GameObject deathParticle;
    public HealthBar healthBar;
    public bool immuniateEnvironmentColdness;
    public bool melt;
    public bool countInActiveChess=true;
    protected override void Awake()
    {
        base.Awake();

        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();
        outline = GetComponent<Outline>();
        GridManager.instance.AddChess(this);
        healthBar = new HealthBar(this);
        healthBar.Hide();
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
    public override void OnPlayerTurnEnd()
    {
        base.OnPlayerTurnEnd();
        DeactiveFreezeFoot();
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
            if(melt)
                Damage(damage);
            ElementReaction(element);
        }
        else
        {
            if (melt)
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
        Debug.Log("Chess:" + gameObject + "Die");
        gameObject.SetActive(false);
        if (deathParticle != null)
            GridFunctionUtility.CreateParticleAt(deathParticle,this);
        Destroy(gameObject);
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
            GChess t = GridManager.instance.GetChess(curLocation);
            if (t|| !GridManager.instance.InRange(curLocation))
            {
                break;
            }
            else
            {
                destination = destination + direction;
            }

        }
        eBeForceMove.Invoke();
        MoveToDirectly(destination);
        DeactiveFreezeFoot();
    }
    public void ThrowTo(Vector2Int destination)
    {
        eBeForceMove.Invoke();
        Debug.Log("ThrowTo" + destination);
        moveComponent.RequestJumpMove(destination);
        location = destination;
        moveComponent.eFinishPath.AddListener(EnterLocation);
        DeactiveFreezeFoot();
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
        eLocationChange.Invoke();
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
        if(element==Element.Ice&&warm)
        {
            DeactiveWarm();
            return;
        }
        base.ElementReaction(element);
        if(element==Element.Fire)
        {
            DeactiveFreezeFoot();
        }
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
    public void FreezeFoot()
    {
        if (freezeFoot)
            return;
        if(prefabFreezenFootVFX!=null)
            freezenFootVFX = GridFunctionUtility.CreateParticleAt(prefabFreezenFootVFX, this);
        freezeFoot = true;
    }
    public void DeactiveFreezeFoot()
    {
        if (!freezeFoot)
            return;
        eFreezeFootBroken.Invoke();
        freezeFoot = false;
        Destroy(freezenFootVFX);
        if(prefabFreezenFootBrokenVFX!=null)
            GridFunctionUtility.CreateParticleAt(prefabFreezenFootBrokenVFX, this);
        freezenFootVFX = null;
    }
    public void Warm()
    {
        if (warm)
            return;
        if (prefabWarmVFX != null)
        {
            warmVFX = GridFunctionUtility.CreateParticleAt(prefabWarmVFX, this);
            warmVFX.transform.parent = render.transform;
        }
        warm = true;
    }
    public void DeactiveWarm()
    {
        if (!warm)
            return;
        warm = false;
        if(warmVFX!=null)
        Destroy(warmVFX);
        warmVFX = null;
        
    }
    override public List<IGetInfo> GetInfos()
    {
        List<IGetInfo> list = new List<IGetInfo>();
        list.AddRange(base.GetInfos());
        list.Add(this);
        if (freezeFoot)
            list.Add(new Information("冻足", "脚被冻住，无法自己移动\n当自己或攻击发起者受到"+UIManager.instance.GetHighTempertureRichText()+"可以解除"));
        if(warm)
            list.Add(new Information("温暖", "抵抗下一次受到的"+UIManager.instance.GetLowTempertureRichText()));
        if (elementComponent.state==ElementState.Frozen)
            list.Add(new Information("冰冻", "无法行动，可通过"+UIManager.instance.GetHighTempertureRichText()+"解除"));
        return list;
    }
    public virtual void MouseEnter()
    {
        outline.enabled = true;
    }
    public virtual void MouseExit()
    {
        outline.enabled = false;
    }
}

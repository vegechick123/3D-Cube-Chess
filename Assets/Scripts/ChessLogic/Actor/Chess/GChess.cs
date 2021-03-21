using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GChess : GActor
{
    public RangeAttributeInt healthAttribute;
    public RangeAttributeInt APAttribute;
    public bool unableAct { get; protected set; }
    public int maxHealth { get { return healthAttribute.maxValue; } }
    //[HideInInspector]
    public int curHealth { get { return healthAttribute.value; } set { healthAttribute.value = value; } }

    public int maxAP { get { return APAttribute.maxValue; } }
    public int curAP { get { return APAttribute.value; } set { APAttribute.value = value; } }
    protected int remainMove = 0;
    static public int recoverAP = 4;
    static public int movePerAp = 2;
    public bool bDead = false;
    public bool lockMove = false;
    public bool floatInAir = false;
    [NonSerialized]
    public ReferenceAttribute unableTomove = new ReferenceAttribute();
    public int curMovement
    {
        get
        {
            if (unableTomove.exist)
                return 0;
            return APAttribute.value * movePerAp + remainMove;
        }
    }
    public List<Modifier> modifiers;
    public int teamID;
    [NonSerialized]
    public UnityEvent eLocationChange = new UnityEvent();
    [NonSerialized]
    public UnityEvent eBeForceMove = new UnityEvent();
    [NonSerialized]
    public UnityEvent eDie = new UnityEvent();
    [NonSerialized]
    public CNavComponent navComponent;
    [NonSerialized]
    public CMoveComponent moveComponent;
    [NonSerialized]
    public Outline outline;


    public HealthBar healthBar;
    protected bool waitShoot;
    public void SpendMoveCost(int distance)
    {
        distance -= remainMove;
        remainMove = 0;
        int APCost = (distance + 1) / movePerAp;
        remainMove = APCost * movePerAp - distance;
        curAP -= APCost;
    }
    public override void GAwake()
    {
        base.GAwake();
        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();
        moveComponent.ePassBy.AddListener((t) =>
        {
            foreach (Modifier m in modifiers)
            {
                m.OnPassFloor(GridManager.instance.GetFloor(t));
            }
        });
        outline = GetComponent<Outline>();
        curHealth = maxHealth;
        healthBar = new HealthBar(this);
        for (int i = 0; i < modifiers.Count; i++)
            modifiers[i] = Instantiate(modifiers[i]);
        foreach (Modifier modifier in modifiers)
        {
            modifier.InitByOwner(this);
        }
        //healthBar.Hide();
    }

    public virtual void OnTurnEnter()
    {
        curAP = recoverAP;
        remainMove = 0;
    }
    public virtual void OnTurnExit()
    {

    }
    public void Recover(int value)
    {
        curHealth += value;
        curHealth = Mathf.Min(curHealth, maxHealth);
    }
    public void Damage(int value)
    {
        curHealth -= value;
        if (curHealth <= 0)
        {
            Die();
        }
        healthBar.Refresh();
    }
    /// <summary>
    /// 坠落
    /// </summary>
    public void TryFall()
    {
        if (!floatInAir)
            Die();
    }
    public void Die()
    {
        render.GetComponent<Animator>().Play("Death");
        bDead = true;
        GridManager.instance.RemoveChess(this);
        eDie.Invoke();
        foreach (Modifier modifier in modifiers)
        {
            modifier.OnDeath();
            modifier.OnEnd();
            Destroy(modifier);
        }
        Destroy(gameObject, 3f);
        Debug.Log("Chess:" + gameObject + "Die");
    }
    #region 位移相关
    /// <summary>
    /// 向一个方向推动这个Chess,如果遇到障碍物则停下
    /// </summary>
    /// <param name="direction">方向，请保证是单位向量</param>
    /// <param name="distance">推动的距离</param>
    async public UniTask PushTowardAsync(Vector2Int direction, int distance = 1)
    {
        Vector2Int destination = location;
        for (int i = 0; i < distance; i++)
        {
            Vector2Int curLocation = destination + direction;
            GChess t = GridManager.instance.GetChess(curLocation);
            if (t || !GridManager.instance.InRange(curLocation))
            {
                break;
            }
            else
            {
                destination = destination + direction;
            }

        }
        eBeForceMove.Invoke();
        await MoveToDirectly(destination);
    }
    async public UniTask ThrowToAsync(Vector2Int destination)
    {
        eBeForceMove.Invoke();
        Debug.Log("ThrowTo" + destination);
        moveComponent.RequestJumpMove(destination);
        location = destination;
        moveComponent.eFinishPath.AddListenerForOnce(EnterLocation);
        await MyUniTaskExtensions.WaitUntilEvent(moveComponent.eFinishPath);
    }
    /// <summary>
    /// 不通过寻路径直走向终点
    /// </summary>
    /// <param name="destination">终点</param>
    async public UniTask MoveToDirectly(Vector2Int destination)
    {
        Debug.Log("MoveToDirectly " + destination);
        moveComponent.RequestDirectMove(destination);
        location = destination;
        moveComponent.eFinishPath.AddListenerForOnce(EnterLocation);
        await MyUniTaskExtensions.WaitUntilEvent(moveComponent.eFinishPath);
    }
    async public UniTask MoveToAsync(GFloor floor)
    {
        await MoveToAsync(floor.location);
    }
    public void AbortMove()
    {
        moveComponent.AbortMove();
    }
    async public virtual UniTask MoveToAsync(Vector2Int destination)
    {
        //Debug.Log("MoveTo " + destination);
        navComponent.GenNavInfo();
        if (navComponent)
        {
            moveComponent.eFinishPath.AddListenerForOnce(EnterLocation);
            location = destination;
            await navComponent.MoveToWtihNavInfo(destination);
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
        EnterLocation();
    }
    public void EnterLocation()
    {
        GridManager.instance.ChessEnterLocation(this);
        eLocationChange.Invoke();
        UIManager.instance.eRefreshFloorHUD.Invoke();
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
    public void FaceToward(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }
    public void FaceToward(Vector2Int dir)
    {
        FaceToward(new Vector3(dir.x, 0, dir.y));
    }
    override public List<IGetInfo> GetInfos()
    {
        List<IGetInfo> list = new List<IGetInfo>();
        list.AddRange(base.GetInfos());
        list.Add(this);

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
    public void AnimationShoot()
    {
        waitShoot = false;
    }
    public virtual void OnPlayerTurnEnd()
    {
        List<Modifier> copy = new List<Modifier>(modifiers);
        foreach (Modifier modifier in copy)
            modifier.OnPlayerTurn();
    }
    public void AddModifier(Modifier modifier, GChess caster = null)
    {
        if (caster == null)
            caster = this;
        modifier = Instantiate(modifier);
        modifier.InitByCaster(caster);
        modifier.InitByOwner(this);
        modifiers.Add(modifier);
    }
    public void RemoveModifier(Modifier modifier)
    {
        modifier.OnEnd();
        Destroy(modifier);
        modifiers.Remove(modifier);
    }
}

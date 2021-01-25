using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GChess : GActor
{
    //TODO:应当移到别处的元素状态及元素粒子效果
    //无法行动
    [HideInInspector]
    public bool freezeFoot=false;
    [HideInInspector]
    public UnityEvent eFreezeFootBroken = new UnityEvent();
    protected GameObject freezenFootVFX;


    public GameObject prefabFreezenFootVFX;

    public GameObject prefabFreezenFootBrokenVFX;

    public bool unableAct { get; protected set; }
    public int health = 3;
    //[HideInInspector]
    public int curHealth { get; protected set; }

    public int movement = 3;
    [HideInInspector]
    public int curMovement;

    public int teamID;
    [HideInInspector]
    public UnityEvent eLocationChange = new UnityEvent();
    [HideInInspector]
    public UnityEvent eBeForceMove = new UnityEvent();
    public UnityEvent eMovementChange = new UnityEvent();
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
    public override void GAwake()
    {
        base.GAwake();

        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();
        outline = GetComponent<Outline>();
        curHealth = health;
        curMovement = movement;
        healthBar = new HealthBar(this);
        healthBar.Hide();
    }

    public virtual void OnTurnEnter()
    {
        curMovement = movement;
    }
    public virtual void OnTurnExit()
    {
        curMovement = movement;
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
            Die();
        }
        healthBar.Refresh();
    }
    public void Die()
    {
        render.GetComponent<Animator>().Play("Death");
        Debug.Log("Chess:" + gameObject + "Die");
        //gameObject.SetActive(false);
        if (deathParticle != null)
            GridExtensions.CreateParticleAt(deathParticle,this);
        GridManager.instance.RemoveChess(this);
        Destroy(gameObject,3f);
    }
    #region 位移相关
    /// <summary>
    /// 向一个方向推动这个Chess,如果遇到障碍物则停下
    /// </summary>
    /// <param name="direction">方向，请保证是单位向量</param>
    /// <param name="distance">推动的距离</param>
    async public UniTask PushTowardAsync(Vector2Int direction, int distance)
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
        await MoveToDirectly(destination);
        DeactiveFreezeFoot();
    }
    async public UniTask ThrowToAsync(Vector2Int destination)
    {
        eBeForceMove.Invoke();
        Debug.Log("ThrowTo" + destination);
        moveComponent.RequestJumpMove(destination);
        location = destination;        
        moveComponent.eFinishPath.AddListener(EnterLocation);
        DeactiveFreezeFoot();
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
        moveComponent.eFinishPath.AddListener(EnterLocation);
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
            moveComponent.eFinishPath.AddListener(EnterLocation);
            location = destination;
            curMovement = 0;
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
        GridManager.instance.GetFloor(location).OnChessEnter(this);
    }
    public void EnterLocation()
    {
        GridManager.instance.GetFloor(location).OnChessEnter(this);
        eLocationChange.Invoke();
        UIManager.instance.eRefreshFloorHUD.Invoke();
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
            freezenFootVFX = GridExtensions.CreateParticleAt(prefabFreezenFootVFX, this);
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
            GridExtensions.CreateParticleAt(prefabFreezenFootBrokenVFX, this);
        freezenFootVFX = null;
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CMoveComponent : Component
{
    public enum MoveState
    {
        Idle,
        Moving,
        Throwing
    }
    public MoveState state { get; private set; }
    public float speed = 1f;
    private float limit = 0.1f;
    private float throwTime = 2f / 3;
    Queue<Vector2Int> path = new Queue<Vector2Int>();
    protected Vector3 lastPosition;
    private Vector3 m_curTargetPosition;
    protected Vector3 curTargetPosition { get { return m_curTargetPosition; } set { lastPosition = transform.position; m_curTargetPosition = value; } }

    [NonSerialized]
    public UnityEvent<Vector2Int> ePassBy = new EventWrapper<Vector2Int>();
    [NonSerialized]
    public UnityEvent eFinishPath = new UnityEvent();
    private float curTime = 0f;

    Vector2Int m_lastLocation;
    Vector2Int lastLocation
    {
        get
        {
            return m_lastLocation;
        }
        set
        {            
            
            m_lastLocation = value;
            lastPosition = GridManager.instance.GetChessPosition3D(value);
        }
    }
    Vector2Int m_curTargetLocation;
    Vector2Int curTargetLocation
    {
        get
        {
            return m_curTargetLocation;
        }
        set
        {
            lastLocation = m_curTargetLocation; 
            m_curTargetLocation = value;
            m_curTargetPosition = GridManager.instance.GetChessPosition3D(value);
        }
    }
    public virtual void AbortMove()
    {
        lastPosition = transform.position;
        curTargetPosition = transform.position;
        state = MoveState.Idle;
    }
    protected virtual void Update()
    {
        if (state != MoveState.Idle)
        {
            curTime += Time.deltaTime;
            UpdateCurTargetPosition();
            if (state == MoveState.Moving)
                MoveForward(Time.deltaTime);
            else if (state == MoveState.Throwing)
                MoveAlong(curTime);
        }

    }
    private void MoveForward(float deltaTime)
    {
        Vector3 vec = curTargetPosition - transform.position;
        float maxDistance = deltaTime * speed;
        //进行移动
        if (vec.magnitude < maxDistance)
        {
            transform.position = curTargetPosition;
        }
        else
        {
            Vector3 dir = vec.normalized;
            transform.position += dir * maxDistance;
        }
    }
    void MoveAlong(float t)
    {
        //t *= speed/2;
        t /= throwTime;
        float height = 3;
        Vector3 targetPosition = Vector3.Lerp(lastPosition, curTargetPosition, t);
        targetPosition.y = height * 2 * t * (1 - t) + curTargetPosition.y;
        transform.position = targetPosition;
    }
    virtual public bool RequestMove(Vector2Int[] pathArr)
    {
        if (state != MoveState.Idle)
        {
            return false;
        }
        else
        {
            path = new Queue<Vector2Int>();
            foreach (var location in pathArr)
            {
                path.Enqueue(location);
            }
            if (path.Count == 0)
            {
                this.InvokeAfter(eFinishPath.Invoke, 1f);
                return false;
            }

            state = MoveState.Moving;
            curTargetLocation = path.Dequeue();
            return true;
        }
    }
    virtual public bool RequestDirectMove(Vector2Int destination)
    {
        return RequestMove(new Vector2Int[] { destination });
    }
    virtual public bool RequestJumpMove(Vector2Int destination)
    {
        if (state != MoveState.Idle)
        {
            return false;
        }
        else
        {
            curTime = 0f;
            state = MoveState.Throwing;
            curTargetLocation = destination;
            lastPosition = transform.position;
            return true;
        }
    }
    protected bool NextPosition()
    {
        curTime = 0;
        transform.position = curTargetPosition;
        ePassBy.Invoke(curTargetLocation);
        //达到最终终点
        if (path.Count == 0)
        {
            lastPosition = curTargetPosition;
            curTargetPosition = transform.position;
            state = MoveState.Idle;
            eFinishPath.Invoke();
            return false;
        }
        else
        {
            Vector2Int next = path.Dequeue();
            //cur
            curTargetLocation = next;
            return true;
        }
    }
    protected virtual void UpdateCurTargetPosition()
    {
        //到达当前的目标位置
        if ((transform.position - curTargetPosition).magnitude < limit && state == MoveState.Moving)
        {
            NextPosition();
        }
        else if (state == MoveState.Throwing && curTime > throwTime)
        {
            NextPosition();
        }
    }
}

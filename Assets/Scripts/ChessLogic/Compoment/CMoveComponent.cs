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
        Moving
    }
    public MoveState state { get; private set; }
    public float speed = 1f;
    private float limit = 0.01f;
    Queue<Vector2Int> path;
    Vector3 curTargetPosition;
    public UnityEvent eFinishPath = new UnityEvent();
    private void Update()
    {
        if(state==MoveState.Moving)
        {
            UpdateCurTargetPosition();
            if (state == MoveState.Moving)
            MoveForward(Time.deltaTime);
        }

    }
    private void MoveForward(float deltaTime)
    {
       Vector3 vec= curTargetPosition -transform.position;
        float maxDistance = deltaTime*speed;
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
    public bool RequestMove(Vector2Int[] pathArr)
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
            state = MoveState.Moving;
            curTargetPosition= GridManager.instance.GetChessPosition3D(path.Dequeue());
            return true;
        }
    }
    void UpdateCurTargetPosition()
    {
        //到达当前的目标位置
        if ((transform.position-curTargetPosition).magnitude<limit)
        {
            //达到最终终点
            if(path.Count==0)
            {
                curTargetPosition = transform.position;
                state = MoveState.Idle;
                eFinishPath.Invoke();
            }
            else
            {
                curTargetPosition = GridManager.instance.GetChessPosition3D(path.Dequeue());
            }
        }
    }
}

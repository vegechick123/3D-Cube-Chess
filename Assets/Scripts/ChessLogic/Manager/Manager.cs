﻿using System;
using System.Collections;
using UnityEngine;
public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; protected set; }
    protected IEnumerator coroutine;//用于异步的协程
    virtual protected void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("存在多个相同的Manager类：" + this.GetType());
            Destroy(this);
        }
        else
        {
            instance = this as T;
            Debug.Log("ManagerCreate:" + this.GetType());
        }

    }
    virtual protected void OnDestroy()
    {
        Debug.Log("ManagerDestory:" + this.GetType());
        instance = null;
    }
    /// <summary>
    /// 执行下一步
    /// </summary>
    public void MoveNext()
    {
        if (GameManager.instance.bEnd)
            return ;
        
        if (coroutine.MoveNext())
        {
            
            if (coroutine.Current != null)
            {
                object t = coroutine.Current;
                switch (t)
                {
                    case float t1:
                        this.InvokeAfter(MoveNext, t1);
                        break;
                    case Func<IEnumerator> f:
                        MoveNext();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
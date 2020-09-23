using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class CommandTaskBase 
{
    public bool bPaused;
    public bool bDone;//终结

    protected GActor castObject;
    protected int curID;//当前执行到哪个参数了
    protected Type[] types;
    protected object[] parameters;

    //只会在任务成功完成时被调用
    public UnityEvent eTaskComplete= new UnityEvent();
    //会在任务成功完成或调用Abort终止时调用
    public UnityEvent eTaskEnd= new UnityEvent();

    protected virtual bool SetCondition<T1>(T1 pa) where T1 : GActor
    {
        return true;
    }
    protected virtual void SetParameters<T1>(T1 pa) where T1 : GActor
    {
        if (types[curID] != typeof(T1))
            return;
        if (bPaused)
            return;
        if (!SetCondition<T1>(pa))
            return;
        parameters[curID++] = pa;
        RefreshInputMode();
        if (curID >= types.Length)
            Finish();
    }

    protected virtual bool CancelParameters()
    {
        if (curID == 0)
            return false;
        parameters[--curID] = null;
        RefreshInputMode();
        return true;
    }
    protected void AddListener(int id)
    {
        if (id < 0 || id >= types.Length)
            return;
        Type t = types[curID];
        switch (true)
        {
            case true when t == typeof(GChess):
                PlayerControlManager.instance.eClickChess.AddListener(SetParameters<GChess>);
                break;
            case true when t == typeof(GFloor):
                PlayerControlManager.instance.eClickFloor.AddListener(SetParameters<GFloor>);
                break;
            default:
                Debug.LogError("意外之外的输入类型");
                break;

        }
    }
    protected void RemoveListener(int id)
    {
        if (id < 0 || id >= types.Length)
            return;
        Type t = types[id];
        switch (true)
        {
            case true when t == typeof(GChess):
                PlayerControlManager.instance.eClickChess.RemoveListener(SetParameters<GChess>);
                break;
            case true when t == typeof(GFloor):
                PlayerControlManager.instance.eClickFloor.RemoveListener(SetParameters<GFloor>);
                break;
            default:
                Debug.LogError("意外之外的输入类型");
                break;

        }
    }
    protected void RefreshInputMode()
    {
        if (curID - 1 >= 0)
        {
            RemoveListener(curID - 1);
        }
        if (curID < types.Length)
        {
            AddListener(curID);
        }
    }
    virtual public void Abort()
    {
        bDone = true;
        RemoveListener(curID);
        eTaskEnd.Invoke();
    }
    abstract protected void Finish();//完成一次参数收集
}

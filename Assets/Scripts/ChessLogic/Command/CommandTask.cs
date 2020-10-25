using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 这是用来收集玩家输入的类
/// 创建之后，会根据传入的task的参数类型，来监听PlayerControlManager的活动，并设置对应的参数
/// 设置完成后，会自动执行传入的task
/// </summary>
public class CommandTask
{

    public bool bPaused;
    public bool bDone;//终结
    protected Delegate task;
    protected GActor castObject;
    protected int curID;//当前执行到哪个参数了
    protected Type[] types;
    protected object[] parameters;
    protected Func<int, object[], bool> checker;
    public UnityEvent<int> eOnHandleParameter = new EventWrapper<int>();
    //只会在任务成功完成时被调用
    public UnityEvent eTaskComplete = new UnityEvent();
    //会在任务成功完成或调用Abort终止时调用
    public UnityEvent eTaskEnd = new UnityEvent();
    public CommandTask(GActor obj,Delegate action,Func<int,object[],bool> _checker=null)
    {
        task = action;
        castObject = obj;
        ParameterInfo[] t= action.GetMethodInfo().GetParameters();
        types = new Type[t.Length];
        for(int i=0;i<t.Length;i++)
        {
            types[i] = t[i].ParameterType;
        }
        //types =action.GetType().GetGenericArguments();
        parameters = new object[types.Length];        
        curID = 0;
        RefreshInputMode();
        checker = _checker;
    }
    protected virtual bool SetCondition<T1>(T1 pa) where T1 : GActor
    {
        return true;
    }
    protected virtual void SetParameters<T1>(T1 pa) where T1 : GActor
    {
        if (types[curID] != typeof(T1))
        {
            Debug.LogError("错误的类型");
            return;
        }
        if (bPaused)
            return;
        object[] temp = (object[])parameters.Clone();
        temp[curID] = pa;
        if (checker != null && !checker.Invoke(curID, temp))
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
        eOnHandleParameter.Invoke(curID);
    }
    virtual public void Abort()
    {
        OnTaskEnd();
    }
    virtual protected void OnTaskEnd()
    {
        bDone = true;
        RemoveListener(curID);
        eTaskEnd.Invoke();
    }
    /// <summary>
    /// 完成一次参数收集
    /// </summary>
    virtual protected void Finish()
    {
        task.DynamicInvoke(parameters);
        eTaskComplete.Invoke();
        OnTaskEnd();
    }
    virtual public void SetPaused(bool t)
    {
        bPaused = t;
    }
}
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
public class InputTask
{

    public bool bPaused;
    public bool bDone;//终结
    protected Action<GActor[]> task;
    protected int curID=-1;//当前执行到哪个参数了
    protected int paramsCount { get { return parameters.Length; } }
    protected GActor[] parameters;
    protected Func<int, GActor, bool> checker;
    public UnityEvent<int> eOnHandleParameter = new EventWrapper<int>();
    //只会在任务成功完成时被调用
    public UnityEvent eTaskComplete = new UnityEvent();
    //会在任务成功完成或调用Abort终止时调用
    public UnityEvent eTaskEnd = new UnityEvent();
    public InputTask(Action<GActor[]> action, int count, Func<int, GActor, bool> _checker = null)
    {
        task = action;
        parameters = new GActor[count];        
        checker = _checker;
    }
    public void Begin()
    {
        curID = 0;
        RefreshInputMode();
    }
    protected virtual bool SetCondition(GActor pa)
    {
        return true;
    }
    protected virtual void SetParameters(GActor pa)
    {

        if (bPaused)
            return;
        if (checker?.Invoke(curID, pa) == false)
            return;
        if (!SetCondition(pa))
            return;
        parameters[curID++] = pa;
        RefreshInputMode();
        if (curID >= paramsCount)
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
    int listenerCnt = 0;
    protected void AddListener()
    {
        if (listenerCnt >= 1)
            return;
        listenerCnt++;
        PlayerControlManager.instance.eClickActor.AddListener(SetParameters);
    }
    protected void RemoveListener()
    {
        if (listenerCnt <= 0)
            return;
        listenerCnt--;        
        PlayerControlManager.instance.eClickActor.RemoveListener(SetParameters);
    }
    protected void RefreshInputMode()
    {
        if (curID == paramsCount)
        {
            RemoveListener();
        }
        if (curID == 0)
        {
            AddListener();
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
        RemoveListener();
        eTaskEnd.Invoke();
    }
    /// <summary>
    /// 完成一次参数收集
    /// </summary>
    virtual protected void Finish()
    {
        task.Invoke(parameters);
        eTaskComplete.Invoke();
        OnTaskEnd();
    }
    virtual public void SetPaused(bool t)
    {
        bPaused = t;
    }

}
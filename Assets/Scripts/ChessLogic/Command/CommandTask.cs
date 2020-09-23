using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommandTask<T> : CommandTaskBase
{
    protected T task;
    public CommandTask(GActor obj,T action)
    {
        task = action;
        castObject = obj;
        types=action.GetType().GetGenericArguments();
        parameters = new object[types.Length];        
        curID = 0;
        RefreshInputMode();
    }
    override protected void Finish()
    {
        bDone = true;
        task.GetType().GetMethod("Invoke").Invoke(task, parameters);
        eTaskEnd.Invoke();
        eTaskComplete.Invoke();
    }
}
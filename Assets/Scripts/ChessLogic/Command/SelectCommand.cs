using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
public class SelectCommand : CommandTask<Action<GChess>>
{
    // Start is called before the first frame update
    public SelectCommand(GActor obj, Action<GChess> action) : base(obj, action)
    {
        
    }
    protected override bool SetCondition<T1>(T1 pa)
    {
        if (!pa.GetComponent<CAgentComponent>())
            return false;
        return true;
    }
    protected override void Finish()
    {
        task.GetType().GetMethod("Invoke").Invoke(task, parameters);
        eTaskComplete.Invoke();
        curID = 0;
        RefreshInputMode();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
public class CyclicTask : InputTask
{
    // Start is called before the first frame update
    public CyclicTask(Action<GActor[]> action,int cnt) : base(action,cnt)
    {
        
    }
    protected override bool SetCondition(GActor pa)
    {
        if (!pa.GetComponent<CAgentComponent>())
            return false;
        return true;
    }
    protected override void Finish()
    {
        task.Invoke(parameters);
        eTaskComplete.Invoke();
        curID = 0;
        RefreshInputMode();
    }
    public static CyclicTask CreateSelectTask()
    {
        return new CyclicTask((t)=> { PlayerControlManager.instance.Select(t[0] as GChess); },1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Manager<AIManager>
{
    [HideInInspector]
    public List<CAICompoment> AIs = new List<CAICompoment>();
    private IEnumerator coroutine;
    public void PreTurn()
    {
        coroutine = PreTurnAIExcute();
        MoveNext();
    }
    IEnumerator PreTurnAIExcute()
    {
        Debug.Log(1);
        foreach (var AI in AIs)
        {
            AI.Visit();
            AI.PerformMove();
            yield return null;
            AI.PrepareSkill();
            yield return null;
        }
    }
    public void PostTurn()
    {
        coroutine = PostTurnAIExcute();
    }
    public IEnumerator PostTurnAIExcute()
    {
        foreach (var AI in AIs)
        {
            AI.PerformSkill();
            yield return null;
        }
    }
    public void MoveNext()
    {
        if(!coroutine.MoveNext())
        {
            coroutine = null;
        }
    }
}

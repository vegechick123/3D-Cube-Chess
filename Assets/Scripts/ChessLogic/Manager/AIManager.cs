using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理AI的单例类
/// 在AIPreTurn和AIPostTurn遍历所有的AIChess,并让他们行动
/// 需要在上一个AI执行完之后才继续下一个AI的执行，因此需要进行异步处理
/// </summary>
public class AIManager : Manager<AIManager>
{
    [HideInInspector]
    public List<CAICompoment> AIs = new List<CAICompoment>();
    private IEnumerator coroutine;//用于异步的协程
    public void PreTurn()
    {
        coroutine = PreTurnAIExcute();
        MoveNext();
    }
    IEnumerator PreTurnAIExcute()
    {
        foreach (var AI in AIs)
        {
            GChess chess=AI.actor as GChess;
            if (chess.unableAct)
                continue;
            AI.Visit();
            AI.PerformMove();
            yield return null;//移动完成后继续执行
            AI.PrepareSkill();
            yield return null;//准备完成后继续执行
        }
        GameManager.instance.AIPreTurnEnd();
    }
    public void PostTurn()
    {
        coroutine = PostTurnAIExcute();
        MoveNext();
    }
    public IEnumerator PostTurnAIExcute()
    {
        foreach (var AI in AIs)
        {
            AI.PerformSkill();
            yield return null;//技能释放完成后继续执行
        }
        StartCoroutine(GridFunctionUtility.InvokeAfter(GameManager.instance.AIPostTurnEnd, 1f));
    }
    /// <summary>
    /// 执行下一步
    /// </summary>
    public void MoveNext()
    {
        //Debug.Log("Next");
        coroutine.MoveNext();
    }
}

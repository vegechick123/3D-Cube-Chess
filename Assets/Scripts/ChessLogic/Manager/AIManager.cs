using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
    private EnemySpawnManager enemySpawn;

    public GameObject curAI;
    protected override void Awake()
    {
        base.Awake();
        enemySpawn = GetComponent<EnemySpawnManager>();
    }
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
            curAI = AI.gameObject;
            AI.Visit();
            var t=UIManager.instance.CreateFloorHUD(AI.actor.location, Color.yellow);
            yield return 1f;
            Destroy(t);
            AI.PerformMove();
            yield return null;//移动完成后继续执行
            AI.PrepareSkill();
            yield return null;//准备完成后继续执行
        }
        IEnumerator spawnCor = enemySpawn.SpawnEnemy();
        while(spawnCor.MoveNext())
        {
            yield return spawnCor.Current;
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
            if ((AI.actor as GChess).unableAct||AI.target==null)
                continue;
            GameObject t = UIManager.instance.CreateFloorHUD(AI.location, Color.yellow);
            AI.PerformSkill();
            yield return null;//技能释放完成后继续执行
            Destroy(t);
            yield return 0.5f;
        }
        this.InvokeAfter(GameManager.instance.AIPostTurnEnd, 1f);
    }
    
}

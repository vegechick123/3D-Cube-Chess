using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
/// <summary>
/// 管理AI的单例类
/// 在AIPreTurn和AIPostTurn遍历所有的AIChess,并让他们行动
/// 需要在上一个AI执行完之后才继续下一个AI的执行，因此需要进行异步处理
/// </summary>
public class AIManager : SingletonMonoBehaviour<AIManager>
{
    [HideInInspector]
    public List<CAICompoment> AIs = new List<CAICompoment>();
    [SerializeField]
    protected List<GChess> markedEnemy;
    public GChess GetSpeceficTarget(int index)
    {
        return markedEnemy[index];
    }
    protected override void Awake()
    {
        base.Awake();
        GridManager.instance.eGridChange.AddListener(RefrshLockState);
    }
    public void RefrshLockState()
    {
        foreach(CAICompoment ai in AIs)
        {
            ai.aiChess.RefreshLockState();
        }
    }
}

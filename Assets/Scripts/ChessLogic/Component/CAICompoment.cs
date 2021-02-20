using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
/// <summary>
/// 这是用附加在GAIChess上的决策组件
/// 它根据Chess的移动范围以及技能的释放范围决定两件事，1.在这个回合移动到哪里，2.以哪个Chess作为技能目标
/// 接着移动到相应位置，并把目标Chess作为参数传递给AISkill，让其做出具体决定
/// </summary>
public class CAICompoment : Component
{
    protected GAIChess aiChess { get { return actor as GAIChess; } }
    public bool postSkillReady=false;
    protected override void Awake()
    {
        base.Awake();
        AIManager.instance.AIs.Add(this);
    }
    /// <summary>
    /// AI根据移动范围以及技能释放范围决定
    /// </summary>
    protected AIPostSkill GetSkill()
    {
        return (actor as GAIChess).postSkill;
    }
    public void CancelSkill()
    {
        aiChess.postSkill.Abort();
    }
    private void OnDestroy()
    {
        Debug.Log(gameObject + "AIDestory");
        if(AIManager.instance)
            AIManager.instance.AIs.Remove(this);
        CancelSkill();
    }
    public async UniTask PreAction()
    {
        GChess target;
        Vector2Int? destination;
        (target, destination) = DecideTargetAndDestination();
        if (destination != null)
            await aiChess.MoveToAsync(destination.Value);
        if(target!=null)
        {
            aiChess.FaceToward((target.location - actor.location).Normalized());
            if (aiChess.preSkill != null)
                await aiChess.preSkill.ProcessAsync(target);
            postSkillReady =  await aiChess.postSkill.Decide(target);
        }
    }
    public async UniTask PostAction()
    {
        if (aiChess.postSkill == null||!postSkillReady)
            return;
        await aiChess.postSkill.ProcessAsync();
        postSkillReady = false;
    }
    protected (GChess,Vector2Int?) DecideTargetAndDestination()
    {
        GChess target = null;
        Vector2Int? destination = null;
        Vector2Int[] moveRange = aiChess.navComponent.GetMoveRangeWithoutOccupy();
        System.Random r = new System.Random();
        moveRange = moveRange.OrderBy(x => r.Next()).ToArray();
        var originLocation = aiChess.location;
        foreach (Vector2Int location in moveRange)
        {
            aiChess.location = location;//暂时更改location以供skill.GetRange来判断
            GChess[] targets = GridManager.instance.GetChesses(GameManager.instance.playerTeam);
            foreach (GChess curTarget in targets)
                if (aiChess.postSkill.GetTargetRange().InRange(curTarget.location))
                {
                    target = curTarget;
                    destination = location;
                    break;
                }
        }
        aiChess.location = originLocation;
        return (target, destination);
    }
}

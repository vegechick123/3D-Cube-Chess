using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CAICompoment : Component
{
    protected GAIChess AIChess { get { return actor as GAIChess; } }
    protected GActor target;
    protected Vector2Int desination;
    protected override void Awake()
    {
        base.Awake();
        AIManager.instance.AIs.Add(this);
    }
    public void Visit()
    {
        AIChess.navComponent.GenNavInfo();
        Vector2Int[] moveRange = AIChess.navComponent.navInfo.range;
        System.Random r = new System.Random();
        moveRange = moveRange.OrderBy(x => r.Next()).ToArray();
        var originLocation = AIChess.location;
        foreach (Vector2Int location in moveRange)
        {
            AIChess.location = location;//暂时更改location以供skill.GetRange来判断
            GChess[] targets = GridManager.instance.GetChesses(GameManager.instance.playerTeam);
            foreach (GChess curTarget in targets)
                if (AIChess.skill.GetRange().InRange(curTarget.location))
                {
                    target = curTarget;
                    desination = location;
                    break;
                }
        }
        AIChess.location = originLocation;


    }
    public void PerformMove()
    {
        AIChess.moveComponent.eFinishPath.AddListener(MoveComplete);
        AIChess.MoveTo(desination);
    }
    private void MoveComplete()
    {
        AIChess.moveComponent.eFinishPath.RemoveListener(MoveComplete);
        AIManager.instance.MoveNext();
    }
    public void PrepareSkill()
    {

    }
    public void PerformSkill()
    {

    }

    protected AISkill GetSkill()
    {
        return (actor as GAIChess).skill;
    }

}

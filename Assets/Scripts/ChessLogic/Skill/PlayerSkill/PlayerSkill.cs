using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class PlayerSkill : Skill
{
    public Sprite icon;
    public string[] cursorHints;
    public int cost = 0;
    public bool infinite = false;
    public int useCount=1;
    public string GetCursorHint(int index)
    {
        if (index >= cursorHints.Length)
            return string.Empty;
        return cursorHints[index];
    }
    public RangeTask CallGetPlayerInput()
    {
        eBegin.Invoke();
        return GetPlayerInput();
    }
    public abstract RangeTask GetPlayerInput();
    public abstract Vector2Int[] GetSelectRange();
    protected RangeTask GetInputTargets(params SkillTarget[] targetType)
    {
        Func<int, GActor, bool> checker =
            (index, target) =>
            {
                switch (targetType[index])
                {
                    case SkillTarget.Chess:
                        return target is GChess;
                    case SkillTarget.Floor:
                        return target is GFloor;
                    case SkillTarget.Actor:
                        return true;
                    case SkillTarget.PlayerChess:
                        return target is GPlayerChess;
                    case SkillTarget.ExceptSelfChess:
                        return target is GChess && target != owner;
                    case SkillTarget.EnemyChess:
                        return target is GAIChess;
                    case SkillTarget.EmptyFloor:
                        return target is GFloor && GridManager.instance.GetChess(target.location) == null;
                    default:
                        throw new NotImplementedException();
                }
            };
        var res = new RangeTask(GetSelectRange, (t) => PlayerControlManager.instance.PerformChessSkill(owner as GPlayerChess, this, t), targetType.Length, checker);
        res.eTaskAbort.AddListener(OnCancel);
        return res;
    }
    protected virtual void OnCancel()
    {
        eEnd.Invoke();
    }
    public enum SkillTarget
    {
        Chess,
        Floor,
        Actor,
        PlayerChess,
        ExceptSelfChess,
        EnemyChess,
        EmptyFloor,
    }
    public virtual bool CanPerform()
    {
        return owner.curAP >= cost;
    }
    public async UniTask CallProcessAsync(GActor[] inputParams)
    {
        owner.curAP -= cost;
        await ProcessAsync(inputParams);
        eEnd.Invoke();
    }
    public abstract UniTask ProcessAsync(GActor[] inputParams);
}
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
    public string GetCursorHint(int index)
    {
        if (index >= cursorHints.Length)
            return string.Empty;
        return cursorHints[index];
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
        return new RangeTask(GetSelectRange,(t)=>_=(owner as GPlayerChess).PerformSkill(this,t),targetType.Length,checker);
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
    public abstract UniTask ProcessAsync(GActor[] inputParams);
}
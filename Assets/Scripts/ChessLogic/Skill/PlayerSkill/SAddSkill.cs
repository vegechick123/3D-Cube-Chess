using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAddSkill : PlayerSkill
{
    public PlayerSkill addSKill;
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(SkillTarget.PlayerChess);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetCircleRange(owner.location, 10);
    }

    async public override UniTask ProcessAsync(GActor[] inputParams)
    {
        GPlayerChess target =inputParams[0] as GPlayerChess;
        target.AddSkill(addSKill);
    }
    public static SAddSkill CreateFromSkill(PlayerSkill skill)
    {
        SAddSkill result = new SAddSkill();
        result.addSKill = skill;
        result.cost = 0;
        result.useCount = 1;
        result.skillVFX = skill.skillVFX;
        return result;
    }
}

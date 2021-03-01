using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/PlayerSkill/STeleport")]
public class STeleport : PlayerSkill
{
    public int range;
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(SkillTarget.ExceptSelfChess, SkillTarget.EmptyFloor);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetCircleRange(owner.location, range);
    }

    async public override UniTask ProcessAsync(GActor[] inputParams)
    {
        GChess targetChess = inputParams[0] as GChess;
        Vector2Int targetLocation = inputParams[1].location;
        await Shoot(targetLocation);
        targetChess.Teleport(targetLocation);
    }
}

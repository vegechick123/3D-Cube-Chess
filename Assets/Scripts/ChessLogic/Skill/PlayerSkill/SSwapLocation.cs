using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/PlayerSkill/SwapLocation ")]
public class SSwapLocation : PlayerSkill
{
    public int range;
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(SkillTarget.Chess, SkillTarget.Chess);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetCircleRange(owner.location, range);
    }

    async public override UniTask ProcessAsync(GActor[] inputParams)
    {
        GChess chessa = inputParams[0] as GChess;
        GChess chessb = inputParams[1] as GChess;
        await Shoot(chessa.location);
        Vector2Int temp=chessa.location;
        chessa.Teleport(chessb.location);
        chessb.Teleport(temp);
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/PlayerSkill/Push")]
public class SPush : PlayerSkill
{
    public SkillTarget targetType = SkillTarget.ExceptSelfChess;
    public int damage = 0;
    public Element element;
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(targetType);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetCircleRange(owner.location,1);
    }

    async public override UniTask ProcessAsync(GActor[] inputParams)
    {
        GChess target = inputParams[0] as GChess;
        Vector2Int direction = target.location-owner.location;
        await ElementSystem.ApplyElementAtAsync(target.location, element, damage);
        await target.PushTowardAsync(direction, 1);

    }

}

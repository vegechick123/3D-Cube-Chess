using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/PlayerSkill/SApplyElementInRange")]
public class SApplyElementInRange : PlayerSkill
{
    public Element element;
    public int selectRange;
    public int affectRange;    
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(SkillTarget.Actor);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetCircleRange(owner.location,selectRange);
    }

    public override async UniTask ProcessAsync(GActor[] inputParams)
    {
        Vector2Int targetLocation = inputParams[0].location;
        await Shoot(targetLocation);
        ElementSystem.ApplyElementAt(targetLocation,element);
    }
}

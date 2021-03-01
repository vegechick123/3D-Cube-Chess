using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillRangeType
{
    Circle,
    Square,
    FourRay
}
[CreateAssetMenu(menuName = "Skill/PlayerSkill/SApplyElementInRange")]
public class SApplyElementInRange : PlayerSkill
{
    public SkillTarget targetType;
    public Element element;
    public int damage;
    public int selectRange;
    public int affectRange;
    public SkillRangeType rangeType=SkillRangeType.Circle; 
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(targetType);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetRangeWithRangeType(owner.location,selectRange,rangeType);
    }                                              

    public override async UniTask ProcessAsync(GActor[] inputParams)
    {
        Vector2Int targetLocation = inputParams[0].location;
        await Shoot(targetLocation);
        await ElementSystem.ApplyElementAtAsync(GridManager.instance.GetCircleRange(targetLocation,affectRange),element,damage);
    }
}

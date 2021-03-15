using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/PlayerSkill/PathBullet")]
public class SPathBullet : PlayerSkill
{
    public SkillTarget targetType;
    public Element element;
    public int damage;
    public int selectRange;
    public int affectRange;
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(targetType);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetRangeWithRangeType(owner.location, selectRange, SkillRangeType.FourRay);
    }

    public override async UniTask ProcessAsync(GActor[] inputParams)
    {
        Vector2Int targetLocation = inputParams[0].location;
        VFXObserver vfxObserver = new VFXObserver();
        vfxObserver.eEnterTile.AddListener((x) =>
            {
                if(x!=0)
                {
                    Vector2Int loc = (targetLocation - owner.location).Normalized()*x+owner.location;
                    ElementSystem.ApplyElementAtAsync(loc, element);
                }
            });
        await Shoot(targetLocation,vfxObserver);
        await ElementSystem.ApplyElementAtAsync(GridManager.instance.GetCircleRange(targetLocation, affectRange), element, damage);
    }
}

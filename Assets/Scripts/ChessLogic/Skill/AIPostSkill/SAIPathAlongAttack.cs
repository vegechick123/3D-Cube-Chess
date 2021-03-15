using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/AIPostSkill/AIPathAlongAttack")]
public class SAIPathAlongAttack : AIPostSkill
{
    public int maxLength = 5;
    public int damage;
    public Element element;
    protected Vector2Int direction;
    public override Vector2Int[] GetTargetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength);
    }
    public override async UniTask<bool> Decide(GChess target)
    {
        if (target == null)
        {
            direction = Vector2Int.zero;
            return false;
        }
        direction = target.location - owner.location;
        direction = direction.Normalized();
        return true;
    }
    async public override UniTask ProcessAsync()
    {
        if (direction == Vector2Int.zero)
            return;
        Vector2Int targetLocation = owner.location+direction;
        await Shoot(targetLocation);
        await ElementSystem.ApplyElementAtAsync(targetLocation,element,damage);
        
    }

    public override Vector2Int[] GetAffectRange()
    {
        return GridManager.instance.GetOneRayRange(owner.location, direction, 1);
    }

}

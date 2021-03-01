using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/AISkill/AIAttackSAIRangeAttack ")]
public class SAILineRangeAttack : AIPostSkill
{
    public int maxLength = -1;
    public int damage;
    public int beginDistance = 1;
    public Element element;
    protected Vector2Int direction;
    public override Vector2Int[] GetTargetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength, beginDistance);
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
        Vector2Int[] range = GetAffectRange();
        Vector2Int targetLocation = range[range.Length - 1];
        await Shoot(targetLocation);
        await ElementSystem.ApplyElementAtAsync(range, element, damage);
    }

    public override Vector2Int[] GetAffectRange()
    {
        return GridManager.instance.GetOneLineRange(owner.location, direction, maxLength,1);
    }

}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum SkillAttackType
{
    flatShot,
    projectile
}
[CreateAssetMenu(menuName = "Skills/AISkill/LockReaction")]
public class SAIAttack : AIPostSkill
{
    public int maxLength = -1;
    public int damage;
    public int beginDistance = 1;
    public Element element;
    public bool willLockTarget;
    protected Vector2Int direction;
    protected Vector2Int offset;
    public SkillAttackType attackType=SkillAttackType.flatShot;
    public override Vector2Int[] GetTargetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength, beginDistance);
    }
    public override async UniTask<bool> Decide(GChess target)
    {
        if (attackType == SkillAttackType.flatShot)
        {
            if (target == null)
            {
                direction = Vector2Int.zero;
                return false;
            }
            direction = target.location - owner.location;
            direction = direction.Normalized();
        }
        else if(attackType == SkillAttackType.flatShot)
        {
            if (target == null)
            {
                offset = Vector2Int.zero;
                return false;
            }
            offset = target.location - owner.location;
        }
        if (willLockTarget)
            LockAt(target);
        return true;
        
    }
    async public override UniTask ProcessAsync()
    {
        Vector2Int[] range = GetAffectRange();
        Vector2Int targetLocation = range[range.Length - 1];
        await Shoot(targetLocation);
        await ElementSystem.ApplyElementAtAsync(targetLocation, element, damage);
        if (willLockTarget)
            ReleaseLock();
    }

    public override Vector2Int[] GetAffectRange()
    {
        switch (attackType)
        {
            case SkillAttackType.flatShot:
                return GridManager.instance.GetOneRayRange(owner.location, direction, -1);
            case SkillAttackType.projectile:
                return new Vector2Int[] { owner.location+offset};
        }
        return null;
    }

}

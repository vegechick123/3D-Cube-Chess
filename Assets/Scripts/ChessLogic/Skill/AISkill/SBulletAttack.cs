using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletAttack", menuName = "Skills/AISkill/BulletAttack")]
public class SBulletAttack : AISkill
{
    public int maxLength = 3;
    public int damage;
    public int beginDistance = 1;
    public Element element;
    protected Vector2Int direction;
    public override Vector2Int[] GetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength, beginDistance);
    }
    public override void Decide(GChess target)
    {
        direction = target.location - owner.location;
        direction = direction.Normalized();

    }
    async public override UniTask Perform()
    {
        
        Vector2Int[] range = GetAffectRange();
        Vector2Int targetPosition = range[range.Length - 1];
        TakeEffect(() =>
        {
            GChess chess = GridManager.instance.GetChess(targetPosition);
            if (chess != null)
            {
                chess.ElementReaction(element);
            }
        }
        , owner.location, targetPosition);
        await base.Perform();
    }

    public override Vector2Int[] GetAffectRange()
    {
        return GridManager.instance.GetOneRayRange(owner.location, direction, maxLength);
    }

    public override UniTask ProcessAsync()
    {
        throw new System.NotImplementedException();
    }
}

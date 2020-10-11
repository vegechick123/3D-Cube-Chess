using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletAttack", menuName = "Skills/AISkill/BulletAttack")]
public class SBulletAttack : AISkill
{
    public int maxLength=3;
    public int damage;
    public Element element;
    protected Vector2Int direction;
    public override Vector2Int[] GetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength);
    }
    public override void Decide(GChess target)
    {
        direction = target.location - owner.location;
        direction = direction.Normalized();

    }
    public override void Perform()
    {
        Vector2Int[] range = GetAffectRange();
        Vector2Int targetPosition = range[range.Length-1];
        GChess chess = GridManager.instance.GetChess(targetPosition);
        if (chess != null)
            chess.ElementReaction(element);
    }
    public override Vector2Int[] GetAffectRange()
    {
        return GridManager.instance.GetOneRayRange(owner.location, direction, maxLength);
    }
}

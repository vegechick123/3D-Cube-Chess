using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletAttack", menuName = "Skills/AISkill/BulletAttack")]
public class SBulletAttack : AIPostSkill
{
    public int maxLength = 3;
    public int damage;
    public int beginDistance = 1;
    public Element element;
    protected Vector2Int direction;
    public override Vector2Int[] GetTargetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength, beginDistance);
    }
    public override async UniTask Decide(GChess target)
    {
        if (target == null)
            direction = Vector2Int.zero;
        direction = target.location - owner.location;
        direction = direction.Normalized();

    }
    async public override UniTask ProcessAsync()
    {
        if (direction == Vector2Int.zero)
            return;
        Vector2Int[] range = GetAffectRange();
        Vector2Int targetPosition = range[range.Length - 1];
        GChess chess = GridManager.instance.GetChess(targetPosition);
        if (chess != null)
        {
            chess.ElementReaction(element);
            Debug.Log(chess.location);
        }
        else
        {
            Debug.Log("Miss");
        }
    }

    public override Vector2Int[] GetAffectRange()
    {
        return GridManager.instance.GetOneRayRange(owner.location, direction, maxLength);
    }

}

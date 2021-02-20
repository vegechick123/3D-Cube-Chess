using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/AISkill/LockShoot")]
public class SLockShoot : AIPostSkill
{
    public int maxLength = 3;
    public int damage;
    public int beginDistance = 1;
    public Element element;
    protected GChess target = null;
    public override Vector2Int[] GetTargetRange()
    {
        return GridManager.instance.GetFourRayRange(owner.location, maxLength, beginDistance);
    }
    public override async UniTask<bool> Decide(GChess target)
    {
        if (!target)
            return false;
        this.target = target;
        return true;
    }
    async public override UniTask ProcessAsync()
    {
        Vector2Int[] range = GetAffectRange();
        Vector2Int targetLocation = range[range.Length - 1];
        await Shoot(targetLocation);
        
        GChess chess = GridManager.instance.GetChess(targetLocation);        
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
        return GridManager.instance.GetRayRange(owner.location, target.location).ToArray();
    }

}

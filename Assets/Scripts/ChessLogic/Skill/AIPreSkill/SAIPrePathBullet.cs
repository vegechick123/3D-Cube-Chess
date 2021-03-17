using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/AIPreSkill/AIPrePathBullet")]
public class SAIPrePathBullet : AIPreSkill
{
    public Element element;
    public async override UniTask ProcessAsync(GChess target)
    {
        Vector2Int targetLocation = target.location;
        VFXObserver vfxObserver = new VFXObserver();
        vfxObserver.eEnterTile.AddListener((x) =>
        {
            if (x != 0)
            {
                Vector2Int loc = (targetLocation - owner.location).Normalized() * x + owner.location;
                ElementSystem.ApplyElementAtAsync(loc, element);
            }
        });
        await Shoot(targetLocation, vfxObserver);
        target.AddModifier(CreateInstance<Damp>(),owner);
        await ElementSystem.ApplyElementAtAsync(targetLocation, element);
    }
}

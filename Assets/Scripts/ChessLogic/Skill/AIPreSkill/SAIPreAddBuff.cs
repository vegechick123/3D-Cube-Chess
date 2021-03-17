using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/AIPreSkill/AIPreAddBuff")]
public class SAIPreAddBuff : AIPreSkill
{
    public Modifier addBuff;
    public override async UniTask ProcessAsync(GChess target)
    {
        await Shoot(target.location);
        target.AddModifier(addBuff,owner);
    }

}

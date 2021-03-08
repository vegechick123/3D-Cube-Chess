using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSkill : Skill
{
    public async UniTask AssignSkill(GPlayerChess target, PlayerSkill skill, PlayerSkill replaceSkill)
    {
        await Shoot(target.location);
        if (replaceSkill == null)
            target.AddSkill(skill);
        else
            target.ReplaceSkill(skill, replaceSkill);
    }
}

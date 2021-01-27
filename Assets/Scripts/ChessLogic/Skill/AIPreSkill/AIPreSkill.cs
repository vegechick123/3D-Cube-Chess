using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIPreSkill : Skill
{
    public abstract UniTask ProcessAsync(GChess target);
}

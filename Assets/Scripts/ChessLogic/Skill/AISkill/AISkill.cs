using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AISkill : Skill
{
    protected abstract void Decide(GChess target);
    protected abstract void Perform();
}

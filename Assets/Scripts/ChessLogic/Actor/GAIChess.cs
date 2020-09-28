using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIChess : GChess
{
    public AISkill skill;
    protected override void Awake()
    {
        base.Awake();
        skill = Instantiate(skill);
        skill.owner = this;
    }
    protected void OnDestroy()
    {
        Destroy(skill);
        skill = null;
    }
}

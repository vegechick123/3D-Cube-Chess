using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIChess : GChess
{
    public AISkill skill;
    protected CAICompoment aiCompoment;
    public override void GAwake()
    {
        base.GAwake();
        aiCompoment = GetComponent<CAICompoment>();
        skill = Instantiate(skill);
        skill.owner = this;
    }
    public override void DisableAction()
    {
        base.DisableAction();
        aiCompoment.CancelSkill();
    }
    public override void ActiveAction()
    {
        base.ActiveAction();
    }
    public override string GetTitle()
    {
        string res = "<color=yellow>";
        res += base.GetTitle();
        res += "</color>";
        return res;
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIChess : GChess
{
    public AIPreSkill preSkill;
    public AIPostSkill postSkill;
    public CAICompoment aiCompoment;
    public override void GAwake()
    {
        base.GAwake();
        aiCompoment = GetComponent<CAICompoment>();
        if (preSkill != null)
        {
            preSkill = Instantiate(preSkill);
            preSkill.Init(this);
        }
        if (postSkill != null)
        {
            postSkill = Instantiate(postSkill);
            postSkill.Init(this);
        }
    }
    public override void DisableAction()
    {
        base.DisableAction();
        aiCompoment.CancelSkill();
    }
    public override string GetTitle()
    {
        string res = "<color=yellow>";
        res += base.GetTitle();
        res += "</color>";
        return res;
    }

}

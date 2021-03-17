using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Modifier/RetargetLockAt")]
public class RetargetLockAt : Modifier
{
    public override void InitByOwner(GChess owner)
    {
        base.InitByOwner(owner);
        IRetargetable skill = (caster as GAIChess).postSkill as IRetargetable;
        if(skill!=null)
        {
            owner.eLocationChange.AddListener(()=>skill?.Retarget());
            caster.eDie.AddListener(EndModifier);
        }
        else
        {
            Debug.LogError("Retarget Error Skill");
        }
    }
    public override void OnPlayerTurn()
    {
        base.OnPlayerTurn();
        EndModifier();
    }
}

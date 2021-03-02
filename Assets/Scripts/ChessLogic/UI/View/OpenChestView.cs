using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenChestView : SkillDisplayView
{
    //protected GChest openChest;
    //ChestOpenTurn currentTurn;
    //void EnterTurn(ChestOpenTurn turn)
    //{
    //    currentTurn = turn;
    //}
    //void ExitTurn()
    //{

    //}
    //public void Bind(GChest chest)
    //{
    //    openChest = chest;
    //    skillButtons = CreatButtonFromSkill(chest.storeSkills);
    //    gameObject.SetActive(true);
    //}
    //public void UnBind()
    //{
    //    openChest = null;
    //    CleanSkillButton();
    //    gameObject.SetActive(false);
    //}
    //public override SkillButton CreatButtonFromSkill(PlayerSkill skill)
    //{
    //    return base.CreatButtonFromSkill(skill);
    //}
    //async UniTask<(PlayerSkill, GPlayerChess, PlayerSkill)> GetAddSkill()
    //{
    //    (GPlayerChess, PlayerSkill) nextReult;
    //    PlayerSkill curResult = null;
    //    nextReult.Item1 = null;
    //    nextReult.Item2 = null;
    //    while (nextReult.Item1 == null)
    //    {
    //        curResult = await MyUniTaskExtensions.WaitUntilEvent(eClickSkill);
    //        if (curResult == null)
    //            return (null, null, null);
    //        nextReult = await GetPlayerChess();
    //    }

    //    return (curResult, nextReult.Item1, nextReult.Item2);
    //}
    //async UniTask<(GPlayerChess, PlayerSkill)> GetPlayerChess()
    //{
    //    PlayerSkill nextReult;
    //    GPlayerChess curResult = null;
    //    nextReult= null;
    //    while (nextReult == null)
    //    {
    //        curResult = await MyUniTaskExtensions.WaitUntilEvent<PlayerSkill>(eClickSkill);
    //        if (curResult == null)
    //            return (null, null, null);
    //        nextReult = await GetPlayerChess();
    //    }

    //    return (curResult, nextReult.Item1, nextReult.Item2);
    //}    

}

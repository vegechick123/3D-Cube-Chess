using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class GTestChess : GChess
{

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnSelect()
    {
        base.OnSelect();
        navComponent.GenNavInfo();
        MoveCommand moveCommand = new MoveCommand(navComponent.navInfo.range, this, MoveTo);
        moveCommand.CreateFloorHUD(new Color(0,1,0,0.5f));
        PlayerControlManager.instance.GenMoveCommand(moveCommand);
        ShowUI();
    }
    protected override void OnDeselect()
    {
        base.OnDeselect();
        HideUI();
    }
    protected void ShowUI()
    {
        UIManager.instance.SwitchSkillButton(skills.ToArray());
    }
    protected void HideUI()
    {
        UIManager.instance.CleanSkillButton();
    }
}

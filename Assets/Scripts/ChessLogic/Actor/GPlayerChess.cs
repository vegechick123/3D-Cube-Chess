using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPlayerChess : GChess
{
    public List<PlayerSkill> skills;
    protected CAgentComponent agentComponent;
    protected override void Awake()
    {
        base.Awake();
        agentComponent = GetComponent<CAgentComponent>();
        if (agentComponent)
        {
            agentComponent.eSelect.AddListener(OnSelect);
            agentComponent.eDeselect.AddListener(OnDeselect);
        }
        foreach (var skill in skills)
        {
            skill.owner = this;
        }
    }
    protected virtual void OnSelect()
    {
        if (curMovement > 0)
        {
            navComponent.GenNavInfo();
            MoveCommand moveCommand = new MoveCommand(navComponent.navInfo.range, this, MoveTo);
            moveCommand.CreateFloorHUD(new Color(0, 1, 0, 0.5f));
            PlayerControlManager.instance.GenMoveCommand(moveCommand);
        }
        ShowUI();
    }
    protected virtual void OnDeselect()
    {
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

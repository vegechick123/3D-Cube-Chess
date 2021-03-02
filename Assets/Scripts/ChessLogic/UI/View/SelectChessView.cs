using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChessView : SkillDisplayView
{
    protected GPlayerChess selectedChess;
    protected bool skillActive = true;
    private void Awake()
    {
        PlayerControlManager.instance.eProcessStart.AddListener(() => skillActive = false);
        PlayerControlManager.instance.eProcessEnd.AddListener(() => skillActive = true);
    }
    public void Bind(GPlayerChess selectedChess)
    {
        this.selectedChess = selectedChess;
        SwitchSkillButton(selectedChess.skills);
        Refresh();
        selectedChess.APAttribute.AddListener(Refresh);
        selectedChess.eSkillChange.AddListener(ReCreateSkillButton);
        PlayerControlManager.instance.eProcessStart.AddListener(Refresh);
        PlayerControlManager.instance.eProcessEnd.AddListener(Refresh);
    }
    public void UnBind()
    {
        selectedChess.APAttribute.RemoveListener(Refresh);
        selectedChess.eSkillChange.RemoveListener(ReCreateSkillButton);
        PlayerControlManager.instance.eProcessStart.RemoveListener(Refresh);
        PlayerControlManager.instance.eProcessEnd.RemoveListener(Refresh);
        CleanSkillButton();
    }
    public void Refresh()
    {
        foreach (SkillButton t in skillButtons)
        {
            if (!skillActive)
                t.button.interactable = false;
            else
                t.button.interactable = t.skill.CanPerform();
        }
    }
    public override SkillButton CreatButtonFromSkill(PlayerSkill skill)
    {
        SkillButton result = base.CreatButtonFromSkill(skill);
        result.button.onClick.AddListener(() => PlayerControlManager.instance.SwitchToSkill(skill));
        return result;
    }
    void ReCreateSkillButton()
    {
        CleanSkillButton();
        SwitchSkillButton(selectedChess.skills); 
        Refresh();
    }
}

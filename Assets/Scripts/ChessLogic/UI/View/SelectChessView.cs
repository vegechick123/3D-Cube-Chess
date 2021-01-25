using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChessView : MonoBehaviour
{
    [NonSerialized]
    public SkillButton[] skillButtons;
    public GameObject prefabSkillButton;
    public GameObject skillBar;
    protected GPlayerChess selectedChess;
    public void Bind(GPlayerChess selectedChess)
    {
        this.selectedChess = selectedChess;
        SwitchSkillButton(selectedChess.skills);
    }
    public void UnBind()
    {
        CleanSkillButton();
    }
    public void RefreshSkill()
    {
        foreach (SkillButton t in skillButtons)
        {

            t.button.interactable = t.skill.cost>=selectedChess.curMovement;
        }
    }
    public void CleanSkillButton()
    {
        if (skillButtons != null)
            foreach (SkillButton skillButton in skillButtons)
            {
                Destroy(skillButton.gameObject);
            }
        skillButtons = null;
    }
    public SkillButton CreatButtonFromSkill(PlayerSkill skill)
    {
        GameObject gameObject = Instantiate(prefabSkillButton, skillBar.transform);
        gameObject.GetComponent<Image>().sprite = skill.icon;
        gameObject.GetComponent<Button>().onClick.AddListener(()=>PlayerControlManager.instance.PreemptSkillTask(skill));
        gameObject.GetComponent<SkillButton>().skill = skill;
        return gameObject.GetComponent<SkillButton>();
    }
    public SkillButton[] CreatButtonFromSkill(List<PlayerSkill> skills)
    {
        SkillButton[] skillbuttons = new SkillButton[skills.Count];
        for (int i = 0; i < skills.Count; i++)
        {
            skillbuttons[i] = CreatButtonFromSkill(skills[i]);
        }
        return skillbuttons;
    }
    public void SwitchSkillButton(List<PlayerSkill> skills)
    {
        CleanSkillButton();
        skillButtons = CreatButtonFromSkill(skills);
    }
    public void DisableSkillButton()
    {
        foreach (SkillButton t in skillButtons)
        {
            t.button.interactable = false;
        }
    }
    public void ActiveSkillButton()
    {
        foreach (SkillButton t in skillButtons)
        {
            t.button.interactable = true;
        }
    }
}

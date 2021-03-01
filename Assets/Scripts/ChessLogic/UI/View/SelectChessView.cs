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
        gameObject.GetComponent<Button>().onClick.AddListener(() => PlayerControlManager.instance.SwitchToSkill(skill));
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
        skillButtons = CreatButtonFromSkill(skills);
    }
    void ReCreateSkillButton()
    {
        CleanSkillButton();
        SwitchSkillButton(selectedChess.skills); 
        Refresh();
    }
}

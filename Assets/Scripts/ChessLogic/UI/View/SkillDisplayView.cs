using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillDisplayView : MonoBehaviour
{
    [NonSerialized]
    public SkillButton[] skillButtons;
    public Button cancelButton;
    public GameObject prefabSkillButton;
    public Transform skillBar;
    public UnityEvent<PlayerSkill> eClickSkill;
    private void Awake()
    {
        cancelButton.onClick.AddListener(()=>OnSkillClick(null));
    }
    public virtual SkillButton CreatButtonFromSkill(PlayerSkill skill)
    {
        GameObject gameObject = Instantiate(prefabSkillButton, skillBar);
        SkillButton result = gameObject.GetComponent<SkillButton>();
        gameObject.GetComponent<Image>().sprite = skill.icon;
        result.button.onClick.AddListener(()=>OnSkillClick(result.skill));
        result.skill = skill;
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
    public void CleanSkillButton()
    {
        if (skillButtons != null)
            foreach (SkillButton skillButton in skillButtons)
            {
                Destroy(skillButton.gameObject);
            }
        skillButtons = null;
    }
    protected virtual void OnSkillClick(PlayerSkill skill)
    {
        eClickSkill.Invoke(skill);
    }
}

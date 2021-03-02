using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectView : SkillDisplayView
{
    bool over = false;
    PlayerSkill selectSkill;
    private void Awake()
    {
        cancelButton.onClick.AddListener(() => SelectSkill(null));
    }
    public async UniTask<PlayerSkill> GetSelectSkill(List<PlayerSkill> skills)
    {
        over = false;
        selectSkill = null;
        SwitchSkillButton(skills);
        gameObject.SetActive(true);
        await UniTask.WaitUntil(()=>over);
        gameObject.SetActive(false);
        var result = selectSkill;
        selectSkill = null;
        return result;
    }
    public override SkillButton CreatButtonFromSkill(PlayerSkill skill)
    {
        SkillButton result =base.CreatButtonFromSkill(skill);
        result.button.onClick.AddListener(() => SelectSkill(result.skill));
        return result;
    }
    void SelectSkill(PlayerSkill skill)
    {
        selectSkill = skill;
        over = true;        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : RangeCommand
{
    PlayerSkill skill;
    public SkillCommand(PlayerSkill skill) : base(skill.GetRange, skill.owner, skill.GetCastAction(),skill.ConditionCheck)
    {
        this.skill = skill;
        CreateFloorHUD(new Color(1, 1, 0, 0.8f));

        eOnHandleParameter.AddListener(SwitchHint);
        UIManager.instance.mouseFollowText.AddHint(4, 0, "右键取消释放技能");
        SwitchHint(0);
        eTaskEnd.AddListener(()=>
        {
            UIManager.instance.mouseFollowText.RemoveHint(1, 0);
            UIManager.instance.mouseFollowText.RemoveHint(4, 0);
        });
    }
    private void SwitchHint(int index)
    {
        UIManager.instance.mouseFollowText.RemoveHint(1, 0);
        UIManager.instance.mouseFollowText.AddHint(1, 0, skill.GetCursorHint(index));
    }
}

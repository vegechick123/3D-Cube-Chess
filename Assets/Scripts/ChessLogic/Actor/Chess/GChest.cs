using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GChest : GPlayerChess
{
    public int turnCount;
    public List<PlayerSkill> storeSkills;
    public override void OnPlayerTurnEnd()
    {
        if (turnCount > 0)
        {
            turnCount--;
            if (turnCount == 0)
                CreateSkillFromStoreSkills();
        }
    }
    void CreateSkillFromStoreSkills()
    {
        skills.Clear();
        for(int i=0;i<storeSkills.Count;i++)
        {
            AddSkill(SAddSkill.CreateFromSkill(storeSkills[i]));
        }

    }
}

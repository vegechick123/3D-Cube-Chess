using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GChest : GChess
{
    public int turnCount;
    public List<PlayerSkill> storeSkills;
    bool complete;
    ChestSkill chestSkill = ScriptableObject.CreateInstance<ChestSkill>();
    public UnityEvent eOpen = new UnityEvent();
    async public UniTask Open()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        eOpen.Invoke();
    }
    public void Close()
    {
        
    }
    async public UniTask AssignSkill(PlayerSkill skill,GPlayerChess target, PlayerSkill replaceSkill)
    {
        await chestSkill.AssignSkill(target, skill,replaceSkill);
    }
}

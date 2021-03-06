﻿using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GChest : GChess
{
    public int roundCount;
    public List<PlayerSkill> storeSkills;
    public SkillVFX chestSkillVFX;
    bool complete;
    ChestSkill chestSkill;
    [NonSerialized]
    public UnityEvent eOpen = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
        chestSkill = ScriptableObject.CreateInstance<ChestSkill>();
        chestSkill.skillVFX = chestSkillVFX;
        chestSkill.Init(this);
    }
    public void Open()
    {
        eOpen.Invoke();
    }
    public void Close()
    {

    }
    async public UniTask AssignSkill(PlayerSkill skill, GPlayerChess target, PlayerSkill replaceSkill)
    {
        await chestSkill.AssignSkill(target, skill, replaceSkill);
    }
}

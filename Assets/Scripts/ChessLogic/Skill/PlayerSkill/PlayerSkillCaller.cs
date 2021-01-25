using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillCaller
{
    public GActor[] inputParams;
    public bool block = true;
    public Func<GActor[],UniTask> pre;
    public Func<GActor[], UniTask> post;
    public UniTask task;
    async UniTask Excute()
    {
        await pre(inputParams);
        await UniTask.WaitWhile(() => block);
        await post(inputParams);
    }
    public void Start()
    {
        task = Excute();
    }
    //public static PlayerSkillCaller CreateFromSkillAndParams(PlayerSkill skill, GActor[] inputParams)
    //{
    //    PlayerSkillCaller res = new PlayerSkillCaller();
    //    res.task=Pla
    //}
}

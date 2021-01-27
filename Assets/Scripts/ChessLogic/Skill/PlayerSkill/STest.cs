using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class STest : PlayerSkill
{
    
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(SkillTarget.Floor);
    }
    public override Vector2Int[] GetSelectRange()
    {
        return GetRangeWithLength(5);
    }

    public async override UniTask ProcessAsync(GActor[] inputParams)
    {
        Debug.Log(inputParams[0].location+"Begin");
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        Debug.Log(inputParams[0].location + "End");
    }
}

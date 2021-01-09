using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestTurn", menuName = "Turn/TestTurn")]
public class TestTurn : Turn
{
    public float intervalTime=3;
    public int debugNumber=5;
    async public override UniTask TurnBehaviourAsync()
    {
        for (int i = 0; i < debugNumber; i++)
        {
            Debug.Log(this + "TurnBehaviour" + intervalTime);
            await UniTask.Delay(TimeSpan.FromSeconds(intervalTime));
        }        
    }
}

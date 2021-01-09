using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIPreTurn", menuName = "Turn/AIPreTurn")]
public class AIPreTurn : Turn
{
    private GameObject curAI;
    async public override UniTask TurnBehaviourAsync()
    {
        foreach (var AI in AIManager.instance.AIs)
        {
            GChess chess = AI.actor as GChess;
            if (chess.unableAct)
                continue;
            curAI = AI.gameObject;
            AI.Visit();
            var t = UIManager.instance.CreateFloorHUD(AI.actor.location, Color.yellow);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Destroy(t);
            await AI.PerformMove();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            AI.PrepareSkill();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }
    }
}
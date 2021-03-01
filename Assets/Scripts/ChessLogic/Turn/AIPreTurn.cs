using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Turn/AIPreTurn")]
public class AIPreTurn : Turn
{
    async public override UniTask TurnBehaviourAsync()
    {
        foreach (var AI in AIManager.instance.AIs)
        {
            GChess chess = AI.actor as GChess;
            if (chess.unableAct)
                continue;            
            var t = UIManager.instance.CreateFloorHUD(AI.actor.location, Color.yellow);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Destroy(t);
            await AI.PreAction();
        }
    }
}
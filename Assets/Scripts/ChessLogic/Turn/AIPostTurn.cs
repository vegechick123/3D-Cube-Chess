using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIPostTurn", menuName = "Turn/AIPostTurn")]
public class AIPostTurn : Turn
{
    async public override UniTask TurnBehaviourAsync()
    {
        foreach (var AI in AIManager.instance.AIs)
        {
            if ((AI.actor as GChess).unableAct || AI.target == null)
                continue;
            GameObject t = UIManager.instance.CreateFloorHUD(AI.location, Color.yellow);
            await AI.PerformSkill();            
            Destroy(t);            
        }        
    }
}

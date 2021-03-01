using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIPostTurn", menuName = "Turn/AIPostTurn")]
public class AIPostTurn : Turn
{
    async public override UniTask TurnBehaviourAsync()
    {
        List<CAICompoment> AIs = new List<CAICompoment>();
        foreach (var AI in AIManager.instance.AIs)
        {
            AIs.Add(AI);
        }
        foreach (var AI in AIs)
        {
            if (AI == null)
                continue;
            if ((AI.actor as GChess).unableAct)
                continue;
            GameObject t = UIManager.instance.CreateFloorHUD(AI.location, Color.yellow);
            await AI.PostAction();
            Destroy(t);
        }
    }
}

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Turn/ChestOpenTurn")]
public class ChestOpenTurn : Turn
{
    [NonSerialized]
    public UnityEvent<GChest> eChestOpen= new EventWrapper<GChest>();
    public async override UniTask TurnBehaviourAsync()
    {
        foreach(GChest target in GridManager.instance.chests)
        {
            if(target.roundCount==turnManager.currentRound)
                await PlayerControlManager.instance.OpenChestAsync(target);
        }
    }
}

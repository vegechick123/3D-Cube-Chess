using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class ChestOpenTurn : Turn
{
    [NonSerialized]
    public UnityEvent<GChest> eChestOpen= new EventWrapper<GChest>();
    public async override UniTask TurnBehaviourAsync()
    {
        foreach(GChest target in GridManager.instance.chests)
        {
            await PlayerControlManager.instance.OpenChestAsync(target);
        }
    }
}

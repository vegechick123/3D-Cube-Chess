using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerTurn", menuName = "Turn/PlayerTurn")]
public class PlayerTurn : Turn
{
    private bool bEnd = false;

    async public override UniTask TurnBehaviourAsync()
    {

        await UniTask.WaitUntil(()=>bEnd);
    }
    public override void OnEnterTurn()
    {
        base.OnEnterTurn();
        bEnd = false;
        PlayerControlManager.instance.PlayerTurnEnter(this);
    }
    public override void OnExitTurn()
    {
        base.OnExitTurn();
        bEnd = true;
    }
    public void EndTurn()
    {
        bEnd = true;
    }
}

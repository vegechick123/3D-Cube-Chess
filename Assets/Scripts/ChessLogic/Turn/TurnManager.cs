using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;
public class TurnManager : MonoBehaviour
{
    [SerializeField]
    protected List<Turn> turnList;

    private bool bStart;
    protected int currentTurnIndex = -1;
    protected Turn currentTurn { get { return turnList[currentTurnIndex]; } }
    public void StartExcute()
    {
        if(bStart)
        {
            Debug.LogError("TurnManager : 重复开始");
            return;
        }
        bStart = true;
        TurnLoopAsync();
    }

    async void TurnLoopAsync()
    {
       while(true)
        {
            GoToNextTurn();
            await currentTurn.TurnBehaviourAsync();
        }
    }

    private void GoToNextTurn()
    {
        //确认当前是否有正在进行的回合
        if (currentTurnIndex >= 0)
        {
            currentTurn.OnExitTurn();
            Debug.Log(currentTurn + "Exit");
        }
        
        currentTurnIndex++;
        if (currentTurnIndex >= turnList.Count)
        {
            currentTurnIndex = 0;
            GameManager.instance.eRoundStart.Invoke();
        }
        currentTurn.Init(this);
        currentTurn.OnEnterTurn();
        Debug.Log(currentTurn + "OnEnter");
    }
}

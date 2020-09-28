using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 控制游戏流程的单例类
/// </summary>
public class GameManager : Manager<GameManager>
{
    
    [NonSerialized]
    public int teamCount = 2;
    [NonSerialized]
    public int playerTeam = 1;
    [NonSerialized]
    public int curRound = 0;
    //游戏回合流程的事件分发
    public UnityEvent eRoundStart = new UnityEvent();
    public UnityEvent eRoundEnd = new UnityEvent();
    public UnityEvent eGameStart = new UnityEvent();
    public UnityEvent eGameEnd = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        GameStart();
    }
    protected void GameStart()
    {
        eGameStart.Invoke();
        RoundStart();
    }
    protected void RoundStart()
    {
        curRound++;
        eRoundStart.Invoke();
        AIPreTurnStart();
    }
    protected void AIPreTurnStart ()
    {
        AIManager.instance.PreTurn();
    }

    public void AIPreTurnEnd()
    {
        PlayerTurnStart();    
    }

    protected void PlayerTurnStart()
    {

    }
    public void PlayerTurnEnd()
    {
        PlayerControlManager.instance.DeSelect();
        AIPostTurnStart();
    }
    protected void AIPostTurnStart()
    {
        AIManager.instance.PostTurn();
    }

    public void AIPostTurnEnd()
    {
        RoundEnd();
    }
    void RoundEnd()
    {
        eRoundEnd.Invoke();
        RoundStart();
    }

    void GameEnd()
    {
        eGameEnd.Invoke();
    }

}

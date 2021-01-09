using Boo.Lang.Runtime.DynamicDispatching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 控制游戏流程的单例类
/// </summary>
// TODO :GameManager需要重构成可拔插流程
public class GameManager : Manager<GameManager>
{
    
    [NonSerialized]
    public int teamCount = 2;
    [NonSerialized]
    public int playerTeam = 1;
    [NonSerialized]
    public int curRound = 0;
    //游戏回合流程的事件分发
    [NonSerialized]
    public bool bEnd;

    public UnityEvent eRoundStart = new UnityEvent();
    public UnityEvent ePlayerTurnBegin = new UnityEvent();
    public UnityEvent ePlayerTurnEnd = new UnityEvent();
    public UnityEvent eGameAwake = new UnityEvent();
    public UnityEvent eGameStart = new UnityEvent();
    public UnityEvent eGameWin = new UnityEvent();
    public UnityEvent eGameLose = new UnityEvent();
    public UnityEvent eGameEnd = new UnityEvent();
    public GameObject startButton;
    public GameObject winUI;
    public bool autoStart = false;
    protected override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        
        GameAwake();
        if (autoStart)
            GameStart();

    }
    protected void GameAwake()
    {
        eGameAwake.Invoke();
    }
    public void GameStart()
    {
        bEnd = false;
        startButton.SetActive(false);
        eGameStart.Invoke();
        RoundStart();
    }
    protected void RoundStart()
    {
        curRound++;
        eRoundStart.Invoke();
        //AIPreTurnStart();
    }


    public void GameWin()
    {
        winUI.SetActive(true);
        eGameWin.Invoke();
        GameEnd();
    }
    public void GameLose()
    {
        eGameLose.Invoke();
        Debug.Log("You Lose");
        GameEnd();
    }
    //TODO :删除，并将对应动作用动画表现
    void GameEnd()
    {
        bEnd = true;
        eGameEnd.Invoke();
        GetComponent<AudioSource>().Stop();
    }

}

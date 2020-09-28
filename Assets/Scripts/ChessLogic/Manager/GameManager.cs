using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : Manager<GameManager>
{
    // Start is called before the first frame update
    [NonSerialized]
    public int teamCount = 2;
    [NonSerialized]
    public int playerTeam = 1;
    [NonSerialized]
    public int curRound = 0;

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
    void GameStart()
    {
        eGameStart.Invoke();
        RoundStart();
    }
    void RoundStart()
    {
        curRound++;
        eRoundStart.Invoke();
        AIPreTurnStart();
    }
    public void AIPreTurnStart ()
    {
        AIManager.instance.PreTurn();
    }

    public void AIPreTurnEnd()
    {
        PlayerTurnStart();    
    }

    public void PlayerTurnStart()
    {

    }
    public void PlayerTurnEnd()
    {

    }
    public void AIPostTurnStart()
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

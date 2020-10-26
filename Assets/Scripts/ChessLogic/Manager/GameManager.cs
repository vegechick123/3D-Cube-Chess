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
    public UnityEvent eRoundEnd = new UnityEvent();
    public UnityEvent ePlayerTurnBegin = new UnityEvent();
    public UnityEvent ePlayerTurnEnd = new UnityEvent();
    public UnityEvent eGameAwake = new UnityEvent();
    public UnityEvent eGameStart = new UnityEvent();
    public UnityEvent eGameWin = new UnityEvent();
    public UnityEvent eGameLose = new UnityEvent();
    public UnityEvent eGameEnd = new UnityEvent();
    public AudioClip audioPlayerTurnBegin;
    public AudioClip audioPlayerTurnEnd;
    public AudioClip audioGameWin;
    public AudioClip audioGameEnd;
    public GameObject startButton;
    public GameObject winUI;
    public ParticleSystem snowWeatherParticle;
    public Color BrightColor;
    public Light sun;
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
        AIPreTurnStart();
    }
    protected void AIPreTurnStart()
    {
        Debug.Log("GameState:AIPreTurnStart");
        StartCoroutine(GridFunctionUtility.InvokeNextFrame(AIManager.instance.PreTurn));
    }

    public void AIPreTurnEnd()
    {
        Debug.Log("GameState:AIPreTurnEnd");

        EnvironmentPreTurnStart();
    }
    protected void EnvironmentPreTurnStart()
    {
        //Debug.Log("GameState:PlayerTurnStart");
        EnvironmentManager.instance.PreTurn();

    }
    public void EnvironmentPreTurnEnd()
    {
        PlayerTurnStart();
    }

    protected void PlayerTurnStart()
    {
        if(audioPlayerTurnBegin)
            AudioSource.PlayClipAtPoint(audioPlayerTurnBegin, transform.position);
        Debug.Log("GameState:PlayerTurnStart");
        ePlayerTurnBegin.Invoke();
        PlayerControlManager.instance.PlayerTurnEnter();
    }
    public void PlayerTurnEnd()
    {
        if (audioPlayerTurnEnd)
            AudioSource.PlayClipAtPoint(audioPlayerTurnEnd, transform.position);
        Debug.Log("GameState:PlayerTurnEnd");
        PlayerControlManager.instance.PlayerTurnExit();
        ePlayerTurnEnd.Invoke();
        coroutine = PlayerTurnEndCor();
        MoveNext();
    }
    public IEnumerator PlayerTurnEndCor()
    {

        GChess[] chesses = GridManager.instance.GetChesses();
        foreach (GChess chess in chesses)
        {
            int t = TempertureManager.instance.GetTempatureAt(chess.location);
            if (t < 0)
            {
                if (!chess.immuniateEnvironmentColdness)
                {
                    chess.ElementDamage(Element.Ice, t - 1);
                    yield return 0.5f;
                }
            }
            else if (t > 0)
            {
                chess.ElementDamage(Element.Fire,t-1);
                yield return 0.5f;
            }
            chess.OnPlayerTurnEnd();
        }
        EnvironmentPostTurnStart();
    }
    protected void EnvironmentPostTurnStart()
    {
        EnvironmentManager.instance.PostTurn();
    }
    public void EnvironmentPostTurnEnd()
    {
        AIPostTurnStart();
    }
    protected void AIPostTurnStart()
    {
        Debug.Log("GameState:AIPostTurnStart");
        StartCoroutine(GridFunctionUtility.InvokeNextFrame(AIManager.instance.PostTurn));
    }

    public void AIPostTurnEnd()
    {
        Debug.Log("GameState:AIPostTurnEnd");
        RoundEnd();
    }
    void RoundEnd()
    {
        eRoundEnd.Invoke();
        RoundStart();
    }
    public void GameWin()
    {
        if(audioGameWin)
            AudioSource.PlayClipAtPoint(audioGameWin, transform.position);
        winUI.SetActive(true);
        snowWeatherParticle.Stop();
        StartCoroutine(WatherClear());
        eGameWin.Invoke();
        GameEnd();
    }
    public void GameLose()
    {
        if (audioGameEnd)
            AudioSource.PlayClipAtPoint(audioGameEnd, transform.position);
        eGameLose.Invoke();
        Debug.Log("You Lose");
        GameEnd();
    }
    IEnumerator WatherClear()
    {
        Color originColor = Camera.main.backgroundColor;
        float originLight = sun.intensity;
        float t = 0;
        while (t < 3)
        {
            Camera.main.backgroundColor = Color.Lerp(originColor, BrightColor, t);
            sun.intensity = Mathf.Lerp(originLight, 1.6f, t);
            t += Time.deltaTime / 3;
            yield return null;
        }
    }
    void GameEnd()
    {
        bEnd = true;
        eGameEnd.Invoke();
        GetComponent<AudioSource>().Stop();
    }

}

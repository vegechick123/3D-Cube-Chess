using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : Manager<GameManager>
{
    // Start is called before the first frame update
    [HideInInspector]
    public int teamCount = 2;
    [HideInInspector]
    public int curTeam = 0;
    [HideInInspector]
    public int playerTeam = 0;

    public UnityEvent[] eTurnStart = new UnityEvent[2];
    public UnityEvent[] eTurnEnd = new UnityEvent[2];
    public UnityEvent eRoundStart = new UnityEvent();
    public UnityEvent eRoundEnd = new UnityEvent();
    public UnityEvent eGameStart = new UnityEvent();
    public UnityEvent eGameEnd = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
        for(int i=0;i<teamCount;i++)
        {
            eTurnStart[i] = new UnityEvent();
            eTurnEnd[i] = new UnityEvent();
        }
    }

    public void NextTurn()
    {
        TurnEnd(curTeam);
        curTeam= (curTeam+1)%2;
        TurnStart(curTeam);
    }
    void TurnStart(int teamID)
    {
        eTurnStart[teamID].Invoke();
    }
    void TurnEnd(int teamID)
    {
        eTurnEnd[teamID].Invoke();
    }
    void RoundStart()
    {
        eRoundStart.Invoke();
    }
    void RoundEnd()
    {
        eRoundEnd.Invoke();
    }
    void GameStart()
    {
        eGameStart.Invoke();
    }
    void GameEnd()
    {
        eGameEnd.Invoke();
    }

}

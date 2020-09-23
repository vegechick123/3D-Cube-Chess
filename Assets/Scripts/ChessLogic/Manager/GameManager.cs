using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : Manager<GameManager>
{
    // Start is called before the first frame update

    public UnityEvent eTurnStart = new UnityEvent();
    public UnityEvent eTurnEnd = new UnityEvent();
    public UnityEvent eGameStart = new UnityEvent();
    public UnityEvent eGameEnd = new UnityEvent();
    void TurnStart()
    {
        eTurnStart.Invoke();
    }
    void TurnEnd()
    {
        eTurnEnd.Invoke();
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

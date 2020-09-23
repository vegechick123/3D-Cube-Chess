using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActor : MonoBehaviour
{
    public Vector2Int location;
    //
    virtual protected void Awake()
    {
        //注册事件
        GameManager.instance.eTurnStart.AddListener(OnTurnStart);
        GameManager.instance.eTurnEnd.AddListener(OnTurnEnd);
        GameManager.instance.eGameStart.AddListener(OnGameStart);
        GameManager.instance.eGameEnd.AddListener(OnGameEnd);
    }
    virtual protected void OnTurnStart()
    {

    }
    virtual protected void OnTurnEnd()
    {

    }
    virtual protected void OnGameStart()
    {

    }
    virtual protected void OnGameEnd()
    {

    }
}

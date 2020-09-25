using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActor : MonoBehaviour
{
    public Vector2Int location;

    virtual protected void Awake()
    {
        //注册事件
        GameManager.instance.eRoundStart.AddListener(OnGameStart);
        GameManager.instance.eRoundEnd.AddListener(OnGameEnd);
        GameManager.instance.eGameStart.AddListener(OnGameStart);
        GameManager.instance.eGameEnd.AddListener(OnGameEnd);
    }
    virtual protected void OnRoundStart()
    {

    }
    virtual protected void OnRoundEnd()
    {

    }
    virtual protected void OnGameStart()
    {

    }
    virtual protected void OnGameEnd()
    {

    }
}

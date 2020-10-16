using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.instance.eGameStart.AddListener(F1);
        GameManager.instance.ePlayerTurnBegin.AddListener(F2);
        GameManager.instance.ePlayerTurnEnd.AddListener(F3);
        GameManager.instance.eGameLose.AddListener(F4);
        GameManager.instance.eGameWin.AddListener(F5);
    }
    void F1()
    {
        Debug.Log("EventTest:游戏开始事件");
    }
    void F2()
    {
        Debug.Log("EventTest:敌方结束事件");
    }
    void F3()
    {
        Debug.Log("EventTest:玩家回合结束事件");
    }
    void F4()
    {
        Debug.Log("EventTest:游戏失败事件");
    }
    void F5()
    {
        Debug.Log("EventTest:游戏胜利事件");
    }
}

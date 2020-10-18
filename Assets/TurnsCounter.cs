using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsCounter : MonoBehaviour
{
    public  Text  roundCounter;
    public int t = 0;
    public void Awake()
    {
      
        GameManager.instance.ePlayerTurnEnd.AddListener(RoundCounter);
    }
    void RoundCounter()
    {
        t++;
        Debug.Log(t);
        roundCounter.text = t.ToString();
        
    }
}

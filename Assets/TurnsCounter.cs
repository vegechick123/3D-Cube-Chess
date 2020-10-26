using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsCounter : MonoBehaviour
{
    public  Text  roundCounter;
    public bool reverse;
    public int t = 0;
    public void Awake()
    {
      
        GameManager.instance.ePlayerTurnEnd.AddListener(RoundCounter);
    }
    void RoundCounter()
    {
        if (!reverse)
            t++;
        else
            t--;
        roundCounter.text = t.ToString();
        
    }
}

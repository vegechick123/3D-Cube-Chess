using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundVictory : MonoBehaviour
{
    public int roundNum;
    private void Awake()
    {
        GameManager.instance.eRoundEnd.AddListener(()=>
        {
            if(GameManager.instance.curRound>=roundNum)
            {
                GameManager.instance.GameWin();
            }
        });
    }
}

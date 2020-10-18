﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITurnBeginUI : MonoBehaviour
{
    // Start is called before the first frame update
    private  float alpha = 0.0f;
    public  float alphaSpeed = 2.0f;
    private CanvasGroup cg;
    public void Awake()
    {
        cg = this.transform.GetComponent<CanvasGroup>();
        GameManager.instance.ePlayerTurnEnd.AddListener(AITurntext);
        gameObject.SetActive(false);

    }
    void Update()
    {
        
        if (alpha != cg.alpha)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, alpha, alphaSpeed * Time.deltaTime);
            if (Mathf.Abs(alpha - cg.alpha) <= 0.01)
            {
                cg.alpha = alpha;                            
            }
        }
       
    }

    public void Hide()
    {
        alpha = 0;

    }
    void AITurntext()
    {
        Hide();
        gameObject.SetActive(true);
        Debug.Log("电脑回合，text改变");
        cg.alpha = 1;    
    }

}

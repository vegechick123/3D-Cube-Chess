using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject selectHint;
    public GChess targetPlayerChess;

    public void Awake()
    {
        GameManager.instance.eGameStart.AddListenerForOnce(SelectHint);

    }
    public void SelectHint()
    {
        selectHint.SetActive(true);
        targetPlayerChess.GetComponent<CAgentComponent>().eSelect.AddListenerForOnce(() =>
        {
            selectHint.SetActive(false);
            MoveHint();
        });

    }
    public GameObject moveHintBegin;
    public GameObject moveHintEnd;
    public void MoveHint()
    {
        moveHintBegin.SetActive(true);
        targetPlayerChess.eLocationChange.AddListenerForOnce(() =>
        {
            moveHintEnd.SetActive(false);
            SkillHint();
        });
    }
    public GameObject skillHintBegin;
    public GameObject skillHintEnd;
    public GChess targetStump; 
    public void SkillHint()
    {
        skillHintBegin.SetActive(true);
        targetStump.eLocationChange.AddListenerForOnce(() =>
        {
            skillHintEnd.SetActive(false);
            TempertureHint1();
        });
    }

    public GameObject tempertureHint1Begin;
    public GameObject tempertureHint1End;
    public Button tempertureButton;
    public void TempertureHint1()
    {
        tempertureHint1Begin.SetActive(true);
        tempertureButton.onClick.AddListenerForOnce(() =>
        {
            tempertureHint1End.SetActive(false);
            TempertureHint2(); 
        });
    }
    public GameObject tempertureHint2Begin;
    public GameObject tempertureHint2End;
    public void TempertureHint2()
    {
        tempertureHint2Begin.SetActive(true);
        tempertureButton.onClick.AddListenerForOnce(() =>
        {
            tempertureHint2End.SetActive(false);
            TurnHint();
        });
    }
    public GameObject turnHint;
    public Button turnEndButton;
    public void TurnHint()
    {
        turnHint.SetActive(true);
        turnEndButton.onClick.AddListenerForOnce(() =>
        {
            turnHint.SetActive(false);
            EnemyHint();
        });
    }
    public GameObject enemyHint;
    public void EnemyHint()
    {
        enemyHint.SetActive(true);
        
    }

}

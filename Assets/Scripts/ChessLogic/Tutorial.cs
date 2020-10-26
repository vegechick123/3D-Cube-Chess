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
    public GameObject moveHint;
    public void MoveHint()
    {
        moveHint.SetActive(true);
        targetPlayerChess.eLocationChange.AddListenerForOnce(() =>
        {
            moveHint.SetActive(false);
            SkillHint();
        });
    }
    public GameObject skillHint;
    public GChess targetStump; 
    public void SkillHint()
    {
        skillHint.SetActive(true);
        targetStump.eLocationChange.AddListenerForOnce(() =>
        {
            skillHint.SetActive(false);
            TempertureHint1();
        });
    }

    public GameObject tempertureHint1;
    public Button tempertureButton;
    public void TempertureHint1()
    {
        tempertureHint1.SetActive(true);
        tempertureButton.onClick.AddListenerForOnce(() =>
        {
            tempertureHint1.SetActive(false);
            TempertureHint2(); 
        });
    }
    public GameObject tempertureHint2;
    public void TempertureHint2()
    {
        tempertureHint2.SetActive(true);
        tempertureButton.onClick.AddListenerForOnce(() =>
        {
            tempertureHint2.SetActive(false);
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

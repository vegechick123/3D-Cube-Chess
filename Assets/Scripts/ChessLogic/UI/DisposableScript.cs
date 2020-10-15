using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisposableScript : MonoBehaviour
{
    public GameObject hintUIBefore;
    public GameObject hintUIAfter;
    private void Start()
    {
        GameManager.instance.ePlayerTurnEnd.AddListenerForOnce(
            () =>
            {
                hintUIBefore.SetActive(false);
                hintUIAfter.SetActive(true);
            });
    }
}

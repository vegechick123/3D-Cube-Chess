using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTransient : Component
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        GameManager.instance.eRoundEnd.AddListener(OnRoundEnd);
    }
    protected void OnRoundEnd()
    {
        (actor as GChess).DieImmediately();
    }
}

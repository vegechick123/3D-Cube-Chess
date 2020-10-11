using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWinWhenBurning : Component
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<CElementComponent>().eStateEnter.AddListener(state =>
        {
            if (state == ElementState.Burning)
                GameManager.instance.GameWin();
        });
    }
}

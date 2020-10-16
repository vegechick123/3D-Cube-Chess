using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 在被冰冻时检查是否全部被冰冻来判断输赢
/// </summary>
public class AllFrozenDefeatCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GetComponent<CElementComponent>().eStateEnter.AddListener(
            (state) =>
            {
                if(state==ElementState.Frozen)
                {
                    if(GridManager.instance.CheckAllPlayerFrozen())
                    {
                        GameManager.instance.GameLose();
                    }
                }
            }
            );
    }
}

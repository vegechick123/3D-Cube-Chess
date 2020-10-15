using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CEventOnSwitchState : Component
{
    // Start is called before the first frame update
    public UnityEvent eOnBurning= new UnityEvent();
    public UnityEvent eOnFrozen = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
        GetComponent<CElementComponent>().eStateEnter.AddListener(state =>
        {
            switch (state)
            {
                case ElementState.Burning:
                    eOnBurning.Invoke();
                    break;
                case ElementState.Normal:
                    break;
                case ElementState.Cold:
                    break;
                case ElementState.Frozen:
                    eOnFrozen.Invoke();
                    break;
                default:
                    break;
            }
        });
    }
}

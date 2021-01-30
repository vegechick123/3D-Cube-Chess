using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class AbilityAttribure<T>
{
    [SerializeField]
    protected T m_value;
    virtual public T value{ get { return m_value; } set{m_value=value;eChange.Invoke();}}
    protected UnityEvent eChange = new UnityEvent();
    public void AddListener(UnityAction t)
    {
        eChange.AddListener(t);
    }
    public void AddListenerForOnce(UnityAction t)
    {
        eChange.AddListenerForOnce(t);
    }
    public void RemoveListener(UnityAction t)
    {
        eChange.RemoveListener(t);
    }
}

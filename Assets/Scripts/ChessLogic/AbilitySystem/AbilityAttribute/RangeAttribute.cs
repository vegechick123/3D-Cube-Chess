using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeAttribute<T> : AbilityAttribure<T> where T : IComparable
{
    [SerializeField]
    protected T m_minValue;
    [SerializeField]
    protected T m_maxValue;
    public T minValue { get { return m_minValue; } }
    public T maxValue { get { return m_maxValue; } }
    public override T value
    {
        set
        {
            m_value = value;
            if (m_value.CompareTo(m_minValue) < 0)
                m_value = m_minValue;
            else if (m_value.CompareTo(m_maxValue) > 0)
                m_value = m_maxValue;
            eChange.Invoke();
        }
    }
}
[Serializable]
public class RangeAttributeInt : RangeAttribute<int> { }
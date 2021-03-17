using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceAttribute : AbilityAttribure<int>
{
    public bool exist
    {
        get
        {
            return value > 0;
        }
    }
    public ReferenceAttribute()
    {
        m_value = 0;
    }
}

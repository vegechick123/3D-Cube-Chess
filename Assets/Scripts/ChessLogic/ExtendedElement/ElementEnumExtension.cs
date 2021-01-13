using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ElementEnumExtension : SingletonScriptableObject<ElementEnumExtension>
{
    [SerializeField]
    private ExtendedElement m_Water=null;
    [SerializeField]
    static public ExtendedElement Water { get { return instance.m_Water; } }

    [SerializeField]
    private ExtendedElement m_Fire = null;
    [SerializeField]
    static public ExtendedElement Fire { get { return instance.m_Fire; } }

    [SerializeField]
    private ExtendedElement m_Ice = null;
    [SerializeField]
    static public ExtendedElement Ice { get { return instance.m_Ice; } }

    [SerializeField]
    private ExtendedElement m_Wind = null;
    [SerializeField]
    static public ExtendedElement Wind { get { return instance.m_Wind; } }

}

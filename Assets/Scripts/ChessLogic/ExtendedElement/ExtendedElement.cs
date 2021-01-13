using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExtendedElement : ScriptableObject
{
    [SerializeField]
    private string m_ElementName = null;// { get; set; }
    public string elementName{ get { return m_ElementName; } }
    [SerializeField]
    private Color m_Color = Color.black;// { get;  set; }
    public Color color { get { return m_Color; } }
    [SerializeField]
    private Sprite m_Icon=null;// { get; set; }
    public Sprite icon { get { return m_Icon; } }
}

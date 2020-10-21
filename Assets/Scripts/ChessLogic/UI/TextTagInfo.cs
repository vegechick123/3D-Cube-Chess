using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextTagInfo
{
    public TextTag tag;
    public string name;
    public Color color;
    public string GetColorfulRichText()
    {
        return name;
    }
}

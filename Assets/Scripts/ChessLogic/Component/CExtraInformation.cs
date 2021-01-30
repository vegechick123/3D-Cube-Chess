using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CExtraInformation : Component,IGetInfo
{
    public string title;
    public string info;
    public string GetTitle()
    {
        return title;
    }
    public string GetInfo()
    {
        return info;
    }

}

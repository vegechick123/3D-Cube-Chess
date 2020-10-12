using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : IGetInfo
{
    public string title;
    public string info;
    public Information(string title,string info)
    {
        this.title = title;
        this.info = info;
    }
    public string GetInfo()
    {
        return info;
    }

    public string GetTitle()
    {
        return title;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel:IChessUI
{
    void IChessUI.Hide()
    {
        throw new System.NotImplementedException();
    }

    void IChessUI.Refresh()
    {
        throw new System.NotImplementedException();
    }

    void IChessUI.Release()
    {
        throw new System.NotImplementedException();
    }

    void IChessUI.Show()
    {
        throw new System.NotImplementedException();
    }

    InfoPanel(GActor actor)
    {
        string title = actor.GetTitle();
        string info = actor.GetInfo();
    }
}

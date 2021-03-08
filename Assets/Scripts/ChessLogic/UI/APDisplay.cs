using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APDisplay : MonoBehaviour
{
    Text text;
    protected GPlayerChess selectedChess;
    void Awake()
    {
        text = GetComponent<Text>();
    }
    public void Bind(GPlayerChess selectedChess)
    {
        this.selectedChess = selectedChess;
        selectedChess.APAttribute.AddListener(Refresh);
        Refresh();
    }
    public void UnBind()
    {
        selectedChess.APAttribute.RemoveListener(Refresh);
        selectedChess = null;
    }
    void Refresh()
    {
        if (!selectedChess)
        {
            text.text = "";
        }
        else
        {
            text.text = $"行动点数：{selectedChess.APAttribute.value}";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseFollowText : MonoBehaviour
{
    public int maxLevel=5;
    private RectTransform rectTransform;
    protected Canvas parentCanvas;
    protected Text text;
    protected SortedList<int, string>[] stringContainer;
    private void Awake()
    {
        text = GetComponent<Text>();
        stringContainer = new SortedList<int, string>[maxLevel];
        for(int i=0;i< stringContainer.Length;i++)
        {
            stringContainer[i] = new SortedList<int, string>();
        }
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }
    public void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentCanvas.transform as RectTransform,
        Input.mousePosition, parentCanvas.worldCamera,
        out movePos);

        transform.position = parentCanvas.transform.TransformPoint(movePos);
    }
    void Refresh()
    {
        text.text = string.Empty;
        foreach (var list in stringContainer)
        {
            if (list.Count == 0)
                continue;
            text.text += list.Values[0];
            text.text += '\n';
        }
    }
    public void AddHint(int level,int priority,string hint)
    {
        stringContainer[level].Add(priority,hint);
        Refresh();
    }
    public void RemoveHint(int level, int priority)
    {
        stringContainer[level].Remove(priority);
        Refresh();
    }
}

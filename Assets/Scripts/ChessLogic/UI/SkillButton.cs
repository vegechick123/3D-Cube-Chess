using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [HideInInspector]
    public Skill skill;

    public void OnPointerEnter(PointerEventData eventData)
    {
        List<IGetInfo> res = new List<IGetInfo>();
        res.Add(skill);
        UIManager.instance.CreateMessage(res);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        List<IGetInfo> res = new List<IGetInfo>();
        UIManager.instance.CreateMessage(res);
    }

}

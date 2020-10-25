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
        UIManager.instance.AddExtraMessage(skill);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.RemoveExtraMessage(skill);
    }

}

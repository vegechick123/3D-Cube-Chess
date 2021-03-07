using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [HideInInspector]
    public PlayerSkill skill;
    public Button button;
    bool show = false;
    void Awake()
    {
        button = GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {        
        UIManager.instance.AddExtraMessage(skill);
        show = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.RemoveExtraMessage(skill);
        show = false;
    }
    private void OnDestroy()
    {
        if(show)
        {
            UIManager.instance.RemoveExtraMessage(skill);
        }
    }

}

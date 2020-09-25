using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    public GameObject prefabFloorHDU;

    public GameObject prefabSkillButton;

    public GameObject[] skillButtons;
    public GameObject skillBar;

       
    public GameObject[] CreateFloorHUD(Vector2Int[] location, Color color)
    {
        GameObject[] gameObject = new GameObject[location.Length];
        for(int i=0;i<location.Length;i++)
        {
            gameObject[i] = CreateFloorHUD(location[i], color);
        }
        return gameObject;
    }
    public GameObject CreateFloorHUD(Vector2Int location,Color color)
    {
        GameObject gameObject= GameObject.Instantiate(prefabFloorHDU, GridManager.instance.GetFloorPosition3D(location) + new Vector3(0, 0.51f, 0),Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        return gameObject;
    }

    public void CleanSkillButton()
    {
        foreach(GameObject gameObject in skillButtons)
        {
            Destroy(gameObject);
        }
    }
    public GameObject CreatButtonFromSkill(Skill skill)
    {
        GameObject gameObject = Instantiate(prefabSkillButton, skillBar.transform);
        gameObject.GetComponent<Image>().sprite = skill.icon;
        gameObject.GetComponent<Button>().onClick.AddListener(skill.CreateCommand);
        return gameObject;
    }
    public GameObject[] CreatButtonFromSkill(Skill[] skills)
    {
        GameObject[] gameObjects = new GameObject[skills.Length];
        for(int i=0;i< skills.Length;i++)
        {
            gameObjects[i] = CreatButtonFromSkill(skills[i]);
        }
        return gameObjects;
    }
    public void SwitchSkillButton(Skill[] skills)
    {
        CleanSkillButton();
        skillButtons = CreatButtonFromSkill(skills);
    }
}

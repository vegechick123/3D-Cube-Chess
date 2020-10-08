using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    public GameObject prefabFloorHDU;
    public GameObject prefabHealthBar;
    public GameObject prefabSkillButton;
    public GameObject prefabImage;

    [NonSerialized]
    public GameObject[] skillButtons;
    public GameObject skillBar;
    public UnityEvent eRefreshFloorHUD = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
        PlayerControlManager.instance.eOverTile.AddListener(OverTile);
    }
    void OverTile(Vector2Int location)
    {
        //鼠标不在一个Tile上的话location的值为（0，-1）
        if(location==Vector2Int.down)
        {
            Debug.Log("Mouse on nothing");
        }
        else
        {
            Debug.Log("Mouse On" + location);
        }
    }
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
    public GameObject CreateHealthBar(GChess chess)
    {
        
        GameObject gameObject = GameObject.Instantiate(prefabHealthBar,chess.render.transform);
        gameObject.GetComponent<GetImage>().Init(chess);

        return gameObject;
    }
   
    public void CleanSkillButton()
    {
        if(skillButtons!=null)
        foreach(GameObject gameObject in skillButtons)
        {
            Destroy(gameObject);
        }
    }
    public GameObject CreatButtonFromSkill(PlayerSkill skill)
    {
        GameObject gameObject = Instantiate(prefabSkillButton, skillBar.transform);
        gameObject.GetComponent<Image>().sprite = skill.icon;
        gameObject.GetComponent<Button>().onClick.AddListener(skill.CreateCommand);
        return gameObject;
    }
    public GameObject[] CreatButtonFromSkill(PlayerSkill[] skills)
    {
        GameObject[] gameObjects = new GameObject[skills.Length];
        for(int i=0;i< skills.Length;i++)
        {
            gameObjects[i] = CreatButtonFromSkill(skills[i]);
        }
        return gameObjects;
    }
    public void SwitchSkillButton(PlayerSkill[] skills)
    {
        CleanSkillButton();
        skillButtons = CreatButtonFromSkill(skills);
    }
    public void DisableSkillButton()
    {
        foreach(GameObject t in skillButtons)
        {
            t.GetComponent<Button>().interactable = false;
        }
    }
    public void ActiveSkillButton()
    {
        foreach (GameObject t in skillButtons)
        {
            t.GetComponent<Button>().interactable = true;
        }
    }

}
